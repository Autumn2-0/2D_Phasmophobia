using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public float temperature = 12;
    public float targetTemperature = 12;
    public float maxTempVariation = 4;
    private float tempVariation = 0;
    public bool GhostRoom = false;
    public bool lights;

    public static List<Room> rooms = new List<Room>();

    public GameObject[] normalLights;
    public GameObject[] eventLights;

    private void Start()
    {
        rooms.Add(this);
    }
    public void SetRoomTemperature(int newTemp)
    {
        targetTemperature += newTemp;

        targetTemperature = Mathf.Clamp(temperature, 15, 65);
    }

    private void Update()
    {
        if (Mathf.Abs((targetTemperature + tempVariation) - temperature) < 0.25)
        {
            temperature = Mathf.Lerp(temperature, targetTemperature + tempVariation, Time.deltaTime);
        }
        else
        {
            tempVariation = Random.Range(-maxTempVariation, maxTempVariation);
        }
    }

    public void toggleLights()
    {
        lights = !lights;
        foreach (GameObject light in normalLights)
            light.SetActive(!light.activeSelf);
    }
    public static void breaker(bool power)
    {
        foreach (Room room in rooms)
        {
            if (power)
            {
                foreach (GameObject light in room.normalLights)
                    light.SetActive(room.lights);
            }
            if (power)
            {
                foreach (GameObject light in room.normalLights)
                    light.SetActive(false);
            }
        }
    }
}