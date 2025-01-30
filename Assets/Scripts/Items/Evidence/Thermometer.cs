using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thermometer : Item
{
    private List<Room> currentRooms = new();
    public Room currentRoom;

    public float inaccuracyMax = 2f;
    private float temp = 20f;
    public float displayTemp;
    private float targetTemp = 20;
    float updateTime = 0f;
    public float resetTime = 0.75f;
    public float adjustSpeed = 0.25f;

    protected override void StartItem()
    {
        displayTemp = temp;
    }

    protected override void UpdateItem()
    {
        //Adjust Temperature Readings based on location
        if (currentRoom != null) //Indoors
        {
            targetTemp = currentRoom.temperature;
        }
        else //Outdoors
        {
            targetTemp = RoomManager.Instance.outdoorTemperature;
        }

        //Adjust Temperature
        temp = Mathf.MoveTowards(temp, targetTemp, Time.deltaTime * adjustSpeed);

        //Update Display Temperature based on Reset Time
        updateTime += Time.deltaTime;
        if (updateTime > resetTime && (!equipped || inHand))
        {
            if (active && inHand)
            {
                //Inaccuracy
                float rand = Random.Range(-1f, 1f);
                displayTemp = temp + ((Mathf.Pow(rand, 3) * inaccuracyMax + rand) /2);

                //Rounding to 2 Decimal Places
                displayTemp *= 100; displayTemp = Mathf.RoundToInt(displayTemp); displayTemp /= 100;

                //Eliminates False Positives
                if (!RoomManager.Instance.breakerOn || targetTemp > 1 || !GameManager.ghost.stats.FreezingTemps)
                {
                    while (displayTemp <= 1)
                        displayTemp += Random.Range(0.01f, inaccuracyMax);
                }

                //Output
                Debug.Log(displayTemp);
            }
            updateTime -= resetTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Insert(0,collision.GetComponent<Room>());
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

    protected override void Interaction()
    {
        return;
    }
}