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

    public GhostStats[] possibleStats;

    public int stepsRemainingUV = 0;
    public bool activeDots;
    public bool activeMS;
    public bool activeGhostHunter;

    [Header("Ghost Activity")]
    private int activityOptions;
    private int activityValue;

    private void Awake()
    {
        stats = possibleStats[Random.Range(0, possibleStats.Length)];
        type = stats.type;
        Debug.Log(type.ToString());

        huntCooldown = stats.huntMaxCooldown;

        GameManager.ghost = this;
        CalculateActivityTotalOptions();
    }

    private void CalculateActivityTotalOptions() //Calculates Total Weights of Current Ghost Activity Options
    {
        activityOptions = 0;
        activityOptions += stats.toggleBreaker;
        activityOptions += stats.disableBreaker;
        activityOptions += stats.toggleLights;
        activityOptions += stats.turnOffLights;
        activityOptions += stats.breakLights;
        activityOptions += stats.sabotageEquipment;
        activityOptions += stats.throwPickup;
        activityOptions += stats.hunt;
        if (stats.Scratching)
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
            activityValue = Random.Range(0, activityOptions);
            ConditionalAction(stats.toggleBreaker, ToggleBreaker);
            ConditionalAction(stats.disableBreaker, DisableBreaker);
            ConditionalAction(stats.toggleLights, ToggleLights);
            ConditionalAction(stats.turnOffLights, TurnOffLights);
            ConditionalAction(stats.breakLights, BreakLights);
            ConditionalAction(stats.sabotageEquipment, SabotageEquipment);
            ConditionalAction(stats.throwPickup, ThrowPickup);
            ConditionalAction(stats.hunt, Hunt);
            if (stats.Scratching)
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
        if (!hunting && huntCooldown <= 0)
        {
            StartCoroutine(HuntSequence());
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
        CalculateActivityTotalOptions();

        //Change to hunting behaviour

        yield return new WaitForSeconds(stats.huntDuration);

        //Change to Roaming behaviour

        hunting = false;
        CalculateActivityTotalOptions();
        RoomManager.Instance.ghostRoom = currentRoom;
        huntCooldown = stats.huntMaxCooldown;
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
        Switch choice = options[Random.Range(0, options.Count)];
        choice.Toggle();
        choice.GhostInteraction(2);
        Debug.Log("The Ghost Toggled Lights");
    }
    private void TurnOffLights()
    {
        List<Switch> options = FindInteractionOptions(Interactable.lights);
        Switch choice = options[Random.Range(0, options.Count)];
        if (choice.isLightsOn())
        {
            choice.Toggle();
            choice.GhostInteraction(3);
            Debug.Log("The Ghost Turned Off Lights");
        }
    } 
    private void BreakLights()
    {
        Debug.Log("The Ghost Broke Lights");
    }
    private void SabotageEquipment()
    {
        Debug.Log("The Ghost Sabotaged Equipment");
    }
    private void ThrowPickup()
    {
        Debug.Log("The Ghost Threw an Object");
    }
    private void Hunt()
    {
        Debug.Log("The Ghost is Hunting");
    }
    private void Scratch()
    {
        Debug.Log("The Ghost Clawed a Surface");
    }
    private void Footprints()
    {
        Debug.Log("The Ghost has left Footprints");
    }
    private void Dots()
    {
        Debug.Log("The Ghost is Visible on Dots");
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