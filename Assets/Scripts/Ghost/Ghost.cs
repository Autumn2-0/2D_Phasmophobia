using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using static UnityEditor.Progress;

public enum GhostType
{
    Spirit,
    Demon,
    Myling,
    Deogen,
    Raiju
}

public class Ghost : MonoBehaviour
{
    [Header("Basic Info")]
    public GhostType type;
    public GhostStats stats;

    [Header("Area")]
    private List<Room> currentRooms = new List<Room>();
    public Room currentRoom;

    public List<Sprite> GhostModels;
    private int spriteID;
    public SpriteRenderer ghostModel;
    public GameObject dotsModel;

    [Header("Roaming Movement")]

    [Header("Hunting Movement")]
    public bool hunting = false;
    public bool detectsPlayer = true;
    public float huntCooldown = 300f;
    private bool ghostCanHunt = false;
    private bool smudged = false;
    private float lastSeenPlayerTime;

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
    }

    private void CalculateActivityTotalOptions() //Calculates Total Weights of Current Ghost Activity Options
    {
        ghostCanHunt = Sanity.Instance.sanity < stats.huntingSanity;

        activityOptions = 0;
        activityOptions += stats.toggleBreaker;
        activityOptions += stats.disableBreaker;
        activityOptions += stats.toggleLights;
        activityOptions += stats.turnOffLights;
        activityOptions += stats.breakLights;
        activityOptions += stats.sabotageEquipment;
        activityOptions += stats.throwPickup;
        if (ghostCanHunt)
            activityOptions += stats.hunt;
        if (stats.Scratching && (ghostCanHunt || stats.highSanityScratching))
        {
            if (hunting)
                activityOptions += stats.huntingScratching;
            else
                activityOptions += stats.passiveScratching;
        }
        if (stats.UV)
            activityOptions += stats.footprints;
        if (stats.Dots && !hunting)
            activityOptions += stats.dots;
        if (stats.GhostWriting && !hunting)
            activityOptions += stats.write;
        if (stats.GhostHunter9000)
            activityOptions += stats.trackable;
        if (stats.Hallucinations)
            activityOptions += stats.hallucination;
    }

    private void GhostActivity()
    {
        bool triggerGhostActivity = false;
        if (hunting)
        {
            if (Random.value * 60f < stats.huntingAPM * Time.deltaTime)
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

        if (triggerGhostActivity)
        {
            CalculateActivityTotalOptions();
            activityValue = Random.Range(0, activityOptions);
            ConditionalAction(stats.toggleBreaker, ToggleBreaker);
            ConditionalAction(stats.disableBreaker, DisableBreaker);
            ConditionalAction(stats.toggleLights, ToggleLights);
            ConditionalAction(stats.turnOffLights, TurnOffLights);
            ConditionalAction(stats.breakLights, BreakLights);
            ConditionalAction(stats.sabotageEquipment, SabotageEquipment);
            ConditionalAction(stats.throwPickup, ThrowPickup);
            ConditionalAction(stats.hunt, Hunt);
            if (stats.Scratching && (ghostCanHunt || stats.highSanityScratching))
            {
                if (hunting)
                    ConditionalAction(stats.huntingScratching, Scratch);
                else
                    ConditionalAction(stats.passiveScratching, Scratch);
            }
            if (stats.UV)
                ConditionalAction(stats.footprints, Footprints);
            if (stats.Dots && !hunting)
                ConditionalAction(stats.dots, Dots);
            if (stats.GhostWriting && !hunting)
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

        switch (hunting)
        {
            case false:
                RoamingMovement();
                break;
            case true:
                HuntingMovement();
                break;
        }
    }

    public void RoamingMovement()
    {

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
        if (Vector2.Distance(transform.position, GameManager.player.transform.position) >= stats.stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.player.transform.position, stats.huntingSpeed * Time.deltaTime);
        }
    }

    public void DetectingPlayer()
    {
        if (GameManager.player.detectableByElectronics && Vector2.Distance(GameManager.player.transform.position, transform.position) < stats.electronicsDetectionRange)
        {
            detectsPlayer = true;
            lastSeenPlayerTime = Time.time;
        }
        else if (StaticInteract.instance.CanReach(GameManager.player.transform.position, transform.position, stats.ghostVisionRange))
        {
            detectsPlayer = true;
            lastSeenPlayerTime = Time.time;
        }
        if (lastSeenPlayerTime + stats.trackingDuration < Time.time)
        {
            detectsPlayer = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Insert(0, collision.GetComponent<Room>());
            currentRoom = currentRooms[0];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Remove(collision.GetComponent<Room>());
            if (currentRooms.Count() > 0)
                currentRoom = currentRooms[0];
            else currentRoom = null;
        }
    }

    public IEnumerator HuntSequence()
    {
        Debug.Log("Started Hunt");
        ghostModel.enabled = true;
        hunting = true;
        smudged = false;
        CalculateActivityTotalOptions();

        //Change to hunting behaviour

        yield return new WaitForSeconds(stats.huntDuration);

        //Change to Roaming behaviour

        hunting = false;
        CalculateActivityTotalOptions();
        RoomManager.Instance.ghostRoom = currentRoom;
        huntCooldown = stats.huntTimer;
        if (smudged)
            huntCooldown += stats.smudgeTimer;
        ghostModel.enabled = false;
        Debug.Log("Finished Hunt");
    }

    private IEnumerator HuntBlinking()
    {
        float waitTime = Random.Range(stats.blinkMinRate, stats.blinkMaxRate);
        yield return new WaitForSeconds(waitTime);

        while (hunting)
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
        Breaker.Instance.GhostInteraction(true);
        Debug.Log("The Ghost Toggled Breaker");
    }
    private void DisableBreaker()
    {
        Breaker.Instance.GhostInteraction(false);
        Debug.Log("The Ghost Disabled Breaker");
    }
    private void ToggleLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        if (options.Count > 0)
        {
            Switch choice = options[Random.Range(0, options.Count)];
            choice.Toggle();
            choice.GhostInteraction(2);
            Debug.Log("The Ghost Toggled Lights");
        }
    }
    private void TurnOffLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        if (options.Count > 0)
        {
            Switch choice = options[Random.Range(0, options.Count)];
            if (choice.isLightsOn())
            {
                choice.Toggle();
                choice.GhostInteraction(3);
                Debug.Log("The Ghost Turned Off Lights");
            }
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
        if (!hunting && huntCooldown <= 0)
        {
            StartCoroutine(HuntSequence());
            StartCoroutine(HuntBlinking());
        }
        Debug.Log("The Ghost is Hunting");
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
            stepsRemainingUV += stats.numberOfFootprints;
            StartCoroutine(UV());
        }
        else
        {
            stepsRemainingUV += stats.numberOfFootprints;
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

        while (elapsedTime < stats.dotsLength && !hunting)
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
}