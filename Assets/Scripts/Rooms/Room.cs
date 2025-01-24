using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public float targetTemperature = 12f;
    public float temperature = 12f; //celcius
    public bool lightsOn;
    public float powerUsage = 10f;

    //Flickering Lights
    private int numberOfLights;
    public static float minInterval = 1f;
    public static float maxInterval = 360f;
    public static float minInactiveDuration = 0.01f;
    public static float maxInactiveDuration = 0.03f;

    public bool isGhostRoom;
    public static float degreesPerSecond = 0.75f;

    public GameObject normalLights;
    public GameObject eventLights;
    public Transform ghostSpawnLocation;

    private void Start()
    {
        //Rooms are now Children of Room Manager, no need for instantiation
        //RoomManager.Instance.InstantiateRoom(this);
        SetLightsActive(false);

        numberOfLights = normalLights.transform.childCount;
        for (int i = 0; i < numberOfLights; i++)
        {
            StartCoroutine(FlickeringLights(i));
        }
    }
    private void Update()
    {
        temperature = Mathf.MoveTowards(temperature, targetTemperature, degreesPerSecond * Time.deltaTime);
    }
    private IEnumerator FlickeringLights(int childIndex)
    {
        while (true)
        {
            // Wait for a random interval
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            // Deactivate the child
            normalLights.transform.GetChild(childIndex).gameObject.SetActive(false);

            // Wait for a random inactive duration
            float inactiveDuration = Random.Range(minInactiveDuration, maxInactiveDuration);
            yield return new WaitForSeconds(inactiveDuration);

            // Reactivate the child
            normalLights.transform.GetChild(childIndex).gameObject.SetActive(true);
        }
    }

    //Sets Starting Temperature
    public void SetStartingTemperature(float newTemp)
    {
        temperature = newTemp;
        targetTemperature = newTemp;
    }

    //Sets Temperature value for the room
    public void SetTargetTemperature(float newTemp)
    {
        targetTemperature = newTemp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Logic for revealing the player's view into the room
        }
        else if (collision.CompareTag("Ghost"))
        {
            //SetTemperature(GameManager.ghost.stats.temperatureModifier);
            //The Temperature is dependent on the Ghost Room, not the Ghosts Position -Andy
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Logic for obscuring the player's view into the room
        }
        else if (collision.CompareTag("Ghost"))
        {
            //SetTemperature(-GameManager.ghost.stats.temperatureModifier);
            //The Temperature is dependent on the Ghost Room, not the Ghosts Position -Andy
        }
    }

    public void SetLightsActive(bool active)
    {
        lightsOn = active;
        UpdateLights();
        UpdateBuildingPower();
    }

    public void ToggleLights()
    {
        lightsOn = !lightsOn;
        UpdateLights();
        UpdateBuildingPower();
    }

    public void BreakLights()
    {
        GetComponentInChildren<Light2D>().intensity = 0.05f;
        powerUsage = 4;
    }

    public void BreakerUpdate()
    {
        UpdateLights();
    }

    public void BreakerFailure()
    {
        lightsOn = false;
        normalLights.SetActive(false);
    }

    public void UpdateBuildingPower()
    {
        RoomManager.Instance.RecalculatePower();
    }

    public float GetCurrentPower()
    {
        if (lightsOn && RoomManager.Instance.breakerOn)
            return powerUsage;
        return 0;
    }

    public void UpdateLights()
    {
        if (RoomManager.Instance.breakerOn)
            normalLights.SetActive(lightsOn);
        else
            normalLights.SetActive(false);
    }
}