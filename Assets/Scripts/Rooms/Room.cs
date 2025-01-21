using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public float temperature = 12f; //celcius
    public bool lightsOn;

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
            SetTemperature(GameManager.ghost.stats.temperatureModifier);
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
            SetTemperature(-GameManager.ghost.stats.temperatureModifier);
        }
    }

    public void SetLightsActive(bool active)
    {
        lightsOn = active;
        normalLights.gameObject.SetActive(active);
    }

    public void toggleLights()
    {
        lightsOn = !lightsOn;
        normalLights.gameObject.SetActive(lightsOn);
    }
}