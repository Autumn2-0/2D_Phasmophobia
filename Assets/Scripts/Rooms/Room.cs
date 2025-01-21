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
    }


    //Add to the Temperature value for the room
    public void SetTemperature(float newTemp)
    {
        temperature += newTemp;

        temperature = Mathf.Clamp(temperature, -6, 12);
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
        normalLights.gameObject.SetActive(active);
    }

    public void ToggleLights()
    {
        lightsOn = !lightsOn;
        if (lightsPowered)
            normalLights.gameObject.SetActive(lightsOn);
        else
            normalLights.gameObject.SetActive(false);
        UpdateBuildingPower();
    }

    public void BreakerUpdate(bool breakerPowered)
    {
        lightsPowered = breakerPowered;
        if (lightsOn && lightsPowered)
        {
            normalLights.gameObject.SetActive(lightsOn);
        }
    }

    public void BreakerFailure()
    {
        lightsPowered = false;
        lightsOn = false;
        normalLights.gameObject.SetActive(false);
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
}