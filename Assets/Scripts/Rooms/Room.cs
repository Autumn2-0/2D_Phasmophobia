using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public float temperature = 12f; //celcius
    public bool lightsOn;
    public bool lightsPowered;
    public float powerUsage = 10f;

    public GameObject normalLights;
    public GameObject eventLights;

    private void Start()
    {
        RoomManager.Instance.InstantiateRoom(this);
        SetLightsActive(false);
    }


    //Sets Temperature value for the room
    public void SetTemperature(float newTemp)
    {
        temperature = newTemp;
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

    public void BreakerUpdate(bool breakerPowered)
    {
        lightsPowered = breakerPowered;
        UpdateLights();
    }

    public void BreakerFailure()
    {
        lightsPowered = false;
        lightsOn = false;
        normalLights.SetActive(false);
    }

    public void UpdateBuildingPower()
    {
        RoomManager.Instance.RecalculatePower();
    }

    public float GetCurrentPower()
    {
        if (lightsOn && lightsPowered)
            return powerUsage;
        return 0;
    }

    public void UpdateLights()
    {
        if (lightsPowered)
            normalLights.SetActive(lightsOn);
        else
            normalLights.SetActive(false);
    }
}