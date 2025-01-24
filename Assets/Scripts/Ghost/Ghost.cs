using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum GhostType
{
    Spirit,
    Demon,
    Myling
}

public class Ghost : MonoBehaviour
{
    [Header("Basic Info")]
    public GhostType type;
    public GhostStats stats;

    [Header("Area")]
    private List<Room> currentRooms = new List<Room>();
    public Room currentRoom;

    [Header("Roaming Movement")]

    [Header("Hunting Movement")]
    public bool hunting = false;
    public float huntCooldown = 300f;
    private bool ghostCanHunt = false;
    private bool smudged = false;

    public GhostStats[] possibleStats;

    private int stepsRemainingUV = 0;
    public GameObject dotsModel;
    public bool activeMS;
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
        if (stats.Dots)
            activityOptions += stats.dots;
        if (stats.GhostWriting)
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
            if (stats.Dots)
                ConditionalAction(stats.dots, Dots);
            if (stats.GhostWriting)
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
        if (Vector2.Distance(transform.position, GameManager.player.transform.position) >= stats.stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.player.transform.position, stats.huntingSpeed * Time.deltaTime);
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
        Debug.Log("Finished Hunt");
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
            if (pickup.GetComponent<Item>() && pickup.GetComponent<Item>().uses < 0)
            {
                pickup.GetComponent<Item>().Use();
                pickup.gameObject.AddComponent<Interaction>().Initiate(3);
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
            if (!choice.GetEquipped())
                choice.GhostInteraction();
            Debug.Log("The Ghost Threw an Object");
        }
    }
    private void Hunt()
    {
        if (!hunting && huntCooldown <= 0)
        {
            StartCoroutine(HuntSequence());
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
        if (!dotsModel.active)
            StartCoroutine(DOTS());
        Debug.Log("The Ghost is Visible on Dots");
    }

    private IEnumerator DOTS()
    {
        dotsModel.SetActive(true);
        yield return new WaitForSeconds(stats.dotsLength);
        dotsModel.SetActive(true);
        yield break;

    }
    private void Write()
    {
        Debug.Log("The Ghost is Writing");
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