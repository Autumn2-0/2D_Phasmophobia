using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public enum GhostType
{
    Spirit,
    Demon,
    Myling,
    Deogen,
    Raiju
}

public enum GhostState
{
    Waiting,
    Roaming,
    Hunting
}

public class Ghost : MonoBehaviour
{
    [Header("Basic Info")]
    public GhostType type;
    public GhostStats stats;
    public GhostState state = GhostState.Waiting;
    private bool GhostActive = false;
    private Player player;
    private Unit unit;

    [Header("Area")]
    private List<Room> currentRooms = new List<Room>();
    public Room currentRoom;

    public List<Sprite> GhostModels;
    private int spriteID;
    public SpriteRenderer ghostModel;
    public GameObject dotsModel;

    [Header("Roaming Movement")]

    [Header("Hunting Movement")]
    public bool detectsPlayer = true;
    public float huntCooldown = 300f;
    private bool ghostCanHunt = false;
    private bool smudged = false;
    private float lastSeenPlayerTime;
    private float stoppingDistance = 0.15f;

    public GhostStats[] possibleStats;
    public bool activeDots = false;
    private int stepsRemainingUV = 0;
    public bool activeMS = false;
    public bool activeGhostHunter;

    //Ghost Activity
    private int activityOptions;
    private int activityValue;

    //Spawning
    public GameObject footprintsPrefab;
    public GameObject scratchesPrefab;

    private void Awake()
    {
        stats = possibleStats[Random.Range(0, possibleStats.Length)];
        type = stats.type;
        Debug.Log(type.ToString());

        huntCooldown = stats.gracePeriod;

        GameManager.ghost = this;
        spriteID = Random.Range(0, GhostModels.Count());
        ghostModel.sprite = GhostModels[spriteID];
        ghostModel.enabled = false;
        player = GameManager.player;
        unit = GetComponent<Unit>();

        if (stats.canPhase)
        {
            Grid.instance.SetWallPenalty(stats.phasingPenalty);
            Grid.instance.GhostCanFade();
        }
    }

    public void Activate()
    {
        if (!GhostActive)
        {
            GhostActive = true;
            state = GhostState.Roaming;
        }
    }

    private void CalculateActivityTotalOptions() //Calculates Total Weights of Current Ghost Activity Options
    {
        ghostCanHunt = Sanity.Instance.sanity < stats.huntingSanity;

        activityOptions = 0;
        activityOptions += stats.toggleBreaker;
        activityOptions += stats.toggleLights;
        activityOptions += stats.breakLights;
        activityOptions += stats.eventLights;
        activityOptions += stats.sabotageEquipment;
        activityOptions += stats.throwPickup;
        if (ghostCanHunt)
            activityOptions += stats.hunt;
        if (stats.Scratching && (ghostCanHunt || stats.highSanityScratching))
        {
            if (state == GhostState.Hunting)
                activityOptions += stats.huntingScratching;
            else
                activityOptions += stats.passiveScratching;
        }
        if (stats.UV)
            activityOptions += stats.footprints;
        if (stats.Dots && state != GhostState.Hunting)
            activityOptions += stats.dots;
        if (stats.GhostWriting && state != GhostState.Hunting)
            activityOptions += stats.write;
        if (stats.GhostHunter9000)
            activityOptions += stats.trackable;
        if (stats.Hallucinations)
            activityOptions += stats.hallucination;
    }

    private void GhostActivity()
    {
        bool triggerGhostActivity = false;
        if (state == GhostState.Hunting)
        {
            if (Random.value * 60f < stats.huntingAPM * Time.deltaTime)
            {
                triggerGhostActivity = true;
            }
        }
        else if (state == GhostState.Roaming)
        {
            if (GameManager.ghost.stats.customActivityNearPlayer && currentRoom == player.currentRoom)
            {
                if (Random.value * 60f < stats.activityNearPlayer * Time.deltaTime)
                {
                    triggerGhostActivity = true;
                }
            }
            else
            {
                if (Random.value * 60f < stats.activityPerMinute * Time.deltaTime)
                {
                    triggerGhostActivity = true;
                }
            }
            
        }

        if (triggerGhostActivity)
        {
            CalculateActivityTotalOptions();
            activityValue = Random.Range(0, activityOptions);
            ConditionalAction(stats.toggleBreaker, ToggleBreaker);
            ConditionalAction(stats.toggleLights, ToggleLights);
            ConditionalAction(stats.breakLights, BreakLights);
            ConditionalAction(stats.eventLights, EventLights);
            ConditionalAction(stats.sabotageEquipment, SabotageEquipment);
            ConditionalAction(stats.throwPickup, ThrowPickup);
            if (ghostCanHunt)
                ConditionalAction(stats.hunt, Hunt);
            if (stats.Scratching && (ghostCanHunt || stats.highSanityScratching))
            {
                if (state == GhostState.Hunting)
                    ConditionalAction(stats.huntingScratching, Scratch);
                else
                    ConditionalAction(stats.passiveScratching, Scratch);
            }
            if (stats.UV)
                ConditionalAction(stats.footprints, Footprints);
            if (stats.Dots && state != GhostState.Hunting)
                ConditionalAction(stats.dots, Dots);
            if (stats.GhostWriting && state != GhostState.Hunting)
                ConditionalAction(stats.write, Write);
            if (stats.GhostHunter9000)
                ConditionalAction(stats.trackable, Trackable);
            if (stats.Hallucinations)
                ConditionalAction(stats.hallucination, Hallucination);
        }
    }

    public void ConditionalAction(int incrementValue, System.Action action)
    {
        if (activityValue >= 0)
        {
            if (activityValue < incrementValue)
                action?.Invoke(); // Execute the function if the condition is true
            activityValue -= incrementValue;
        }
    }

    private void Update()
    {

        GhostActivity();
        
        
        if (huntCooldown > 0)
        {
            huntCooldown -= Time.deltaTime;
        }

        switch (state)
        {
            case GhostState.Waiting:
                unit.speed = 0.01f;
                break;
            case GhostState.Roaming:
                RoamingMovement();
                break;
            case GhostState.Hunting:
                HuntingMovement();
                break;
        }
    }
    public void RoamingMovement()
    {
        if (unit.ReachedDestination(stoppingDistance))
        {
            if (Random.value < stats.replaceRoom)
            {
                RoomManager.Instance.ChangeGhostRoom(currentRoom);
            }
            if (Random.value < stats.returnToRoom)
            {
                SetTarget(RoomManager.Instance.ghostRoom.GetRandomPointInRoom());
            }
            else
            {
                while (true)
                {
                    Vector2 newPos = (Vector2)transform.position + Random.insideUnitCircle * stats.roamingRange;
                    if (RoomManager.Instance.IsPointInRoom(newPos))
                    {
                        SetTarget(newPos);
                        break;
                    }
                }
            }
        }
        unit.speed = stats.defaultSpeed - unit.GetMovementPenalty() * (stats.defaultSpeed - stats.phasingSpeed)/stats.phasingPenalty;
    }

    public void HuntingMovement()
    {
        //ElectronicsBoost
        bool boosted = false;
        if (stats.electronicsBoostSpeed)
        {
            foreach (Item item in Interactable.electronics)
            {
                if (StaticInteract.instance.CanReach(transform.position, item.transform.position, stats.electronicsBoostRange, stats.reachThroughWalls)) // Check if the item meets the condition
                {
                    boosted = true; break;
                }
            }
        }
        DetectingPlayer();
        
        //Need to Add Pathfinding. If boosted, hunting speed += stats.electronicsSpeedBoost
        if (Vector2.Distance(transform.position, player.transform.position) >= stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, stats.chasingSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Vector2 pos)
    {
        unit.target.transform.SetParent(null);
        unit.target.transform.position = pos;
    }
    public void ChaseTarget()
    {
        unit.target.transform.SetParent(player.transform);
        unit.target.transform.localPosition = Vector2.zero;
    }

    public void DetectingPlayer()
    {
        if (player.detectableByElectronics && Vector2.Distance(player.transform.position, transform.position) < stats.electronicsDetectionRange)
        {
            detectsPlayer = true;
            lastSeenPlayerTime = Time.time;
        }
        else if (StaticInteract.instance.CanReach(player.transform.position, transform.position, stats.ghostVisionRange))
        {
            detectsPlayer = true;
            lastSeenPlayerTime = Time.time;
        }
        if (lastSeenPlayerTime + stats.trackingDuration < Time.time)
        {
            detectsPlayer = false;
        }
    }

    public IEnumerator HuntSequence()
    {
        Debug.Log("Started Hunt");
        state = GhostState.Waiting;
        RoomManager.Instance.LockDoor();

        yield return new WaitForSeconds(stats.preHuntTimer);

        ghostModel.enabled = true;
        state = GhostState.Hunting;
        smudged = false;
        CalculateActivityTotalOptions();

        //Change to hunting behaviour

        yield return new WaitForSeconds(stats.huntDuration);

        //Change to Roaming behaviour

        state = GhostState.Roaming;
        CalculateActivityTotalOptions();
        huntCooldown = stats.huntCooldown;
        if (smudged)
            huntCooldown += stats.smudgeTimer;
        ghostModel.enabled = false;
        RoomManager.Instance.HuntLightsReset();
        Debug.Log("Finished Hunt");
    }

    private IEnumerator HuntBlinking()
    {
        float waitTime = Random.Range(stats.blinkMinRate, stats.blinkMaxRate);
        yield return new WaitForSeconds(waitTime);

        while (state == GhostState.Hunting)
        {
            if (ghostModel.enabled && 2 * Random.value < stats.visibilityToggleChance + (0.5f - stats.huntingVisibleChance))
            {
                ghostModel.enabled = !ghostModel.enabled;
            }
            else if (!ghostModel.enabled && 2 * Random.value < stats.visibilityToggleChance + (stats.huntingVisibleChance - 0.5f))
            {
                ghostModel.enabled = !ghostModel.enabled;
            }

            waitTime = Random.Range(stats.blinkMinRate, stats.blinkMaxRate);
            yield return new WaitForSeconds(waitTime);
        }

        ghostModel.enabled = false;
    }

    public List<T> FindInteractionOptions<T>(List<T> items) where T : Component
    {
        List<T> resultList = new List<T>(); // Clear the result list before adding filtered items

        foreach (T item in items)
        {
            if (StaticInteract.instance.CanReach(transform.position, item.transform.position, stats.ghostReach, stats.reachThroughWalls)) // Check if the item meets the condition
            {
                resultList.Add(item); // Add the item to the result list if it meets the condition
            }
        }
        return resultList;
    }

    //Ghost Activity Functions
    private void ToggleBreaker() 
    {
        Breaker.Instance.GhostInteraction(GameManager.ghost.stats.onlyTurnBreakerOff);
        Debug.Log("The Ghost Toggled Breaker");
    }
    private void ToggleLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        if (options.Count > 0)
        {
            Switch choice = options[Random.Range(0, options.Count)];
            if (GameManager.ghost.stats.onlyTurnLightsOff)
                choice.room.SetLightsActive(false);
            else
                choice.Toggle();
            choice.GhostInteraction(2);
            Debug.Log("The Ghost Toggled Lights");
        }
    }
    private void BreakLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        if (options.Count > 0)
        {
            Switch choice = options[Random.Range(0, options.Count)];
            choice.room.BreakLights();
            choice.GhostInteraction(4);
            Debug.Log("The Ghost Broke Lights");
        }
    }
    private void EventLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        if (options.Count > 0)
        {
            Switch choice = options[Random.Range(0, options.Count)];
            choice.room.EventLights();
            choice.GhostInteraction(3);
            Debug.Log("Ghost Event");
        }
    }
    private void SabotageEquipment()
    {
        List<PickUp> options = FindInteractionOptions(Interactable.physicsObjects);
        foreach (PickUp pickup in options)
        {
            if (pickup.GetComponent<Item>() && pickup.GetComponent<Item>().uses < 0 && !pickup.equipped)
            {
                pickup.GetComponent<Item>().Use();
                InteractionMarking.Instantiate(pickup.gameObject, 3);
                Debug.Log("The Ghost Sabotaged Equipment");
                return;
            }
        }
    }
    private void ThrowPickup()
    {
        List<PickUp> options = FindInteractionOptions(Interactable.physicsObjects);
        if (options.Count > 0)
        {
            PickUp choice = options[Random.Range(0, options.Count)];
            choice.GhostInteraction();
            Debug.Log("The Ghost Threw an Object");
        }
    }
    private void Hunt()
    {
        if (state != GhostState.Hunting && huntCooldown <= 0 && player.currentRoom != null)
        {
            //Hunt Blocked
            foreach (Crucifix crucifix in Interactable.crucifixs)
            {
                if (crucifix.uses > 0 && StaticInteract.instance.CanReach(transform.position, crucifix.transform.position, crucifix.range, crucifix.throughWalls || stats.reachThroughWalls))
                {
                    crucifix.uses--;
                    return;
                }
            }

            //Not Blocked
            StartCoroutine(HuntSequence());
            StartCoroutine(HuntBlinking());
        }
        Debug.Log("The Ghost is Hunting");
    }

    public void TriggerEarlyHunt()
    {
        huntCooldown = -1;
        Hunt();
    }
    private void Scratch()
    {
        Instantiate(scratchesPrefab, transform.position, transform.rotation);
        Debug.Log("The Ghost Clawed a Surface");
    }
    private void Footprints()
    {
        if (stepsRemainingUV == 0)
        {
            stepsRemainingUV += 3 + stats.numberOfFootprints;
            StartCoroutine(UV());
        }
        else
        {
            stepsRemainingUV += 3 + stats.numberOfFootprints;
        }
        Debug.Log("The Ghost is leaving Footprints");
    }

    private IEnumerator UV()
    {
        while (stepsRemainingUV > 0)
        {
            // Spawn the prefab at the position and rotation of the GameObject this script is attached to
            Instantiate(footprintsPrefab, transform.position, transform.rotation);
            stepsRemainingUV--;

            // Wait for the specified interval before the next spawn
            yield return new WaitForSeconds(Random.Range(0.6f,0.8f));
        }
        yield break;
    }

    private void Dots()
    {
        if (!activeDots)
        {
            activeDots = true;
            StartCoroutine(DOTS());
            Debug.Log("The Ghost is Visible on Dots");
        }
    }

    private IEnumerator DOTS()
    {
        float elapsedTime = 0f;

        while (elapsedTime < stats.dotsLength && state != GhostState.Hunting)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        activeDots = false;
    }

    public void SetDotsVisibility(float percentVisible)
    {
        if (activeDots && percentVisible > 0 && (!stats.dotsRequireCamera || GhostOrbs.inUse))
        {
            dotsModel.SetActive(true);
            Color c = dotsModel.GetComponent<SpriteRenderer>().color; c.a = percentVisible;
            c = dotsModel.GetComponent<SpriteRenderer>().color = c;
            dotsModel.GetComponent<Light2D>().intensity = percentVisible;
        }
        else
        {
            dotsModel.SetActive(false);
        }
    }
    private void Write()
    {
        List<GhostWriting> options = FindInteractionOptions(Interactable.books);
        if (options.Count > 0)
        {
            PickUp choice = options[Random.Range(0, options.Count)];
            InteractionMarking.Instantiate(choice.gameObject, choice.GhostInteraction(true));
            Debug.Log("The Ghost is Writing");
        }
    }
    private void Trackable()
    {
        Debug.Log("The Ghost is Trackable");
    }
    private void Hallucination()
    {
        Debug.Log("The Ghost is Haunting You");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Insert(0, collision.GetComponent<Room>());
            currentRoom = currentRooms[0];
            if (stats.currentRoomFreezes)
                currentRoom.SetTargetTemperature(stats.freezingRoomTemp);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            if (stats.currentRoomFreezes && !currentRoom.isGhostRoom)
                RoomManager.Instance.UpdateTemperature(collision.GetComponent<Room>());
            currentRooms.Remove(collision.GetComponent<Room>());
            if (currentRooms.Count() > 0)
            {
                currentRoom = currentRooms[0];
            }
            else currentRoom = null;
        }
    }
}