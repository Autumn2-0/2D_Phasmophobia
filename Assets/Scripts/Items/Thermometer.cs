using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : Item
{
    public List<Room> currentRoom = new List<Room>();

    public float inaccuracyMax = 2f;
    public float temp = 20f;
    public float displayTemp;
    public float targetTemp = 20;
    float updateTime = 0f;
    public float resetTime;
    public float adjustSpeed = 0.075f;

    private void Awake()
    {
        displayTemp = temp;
    }

    protected override void UpdateItem()
    {
        //Adjust Temperature Readings based on location
        if (currentRoom[0] != null) //Indoors
        {
            targetTemp = currentRoom[0].temperature;
        }
        else //Outdoors
        {
            targetTemp = RoomManager.Instance.outdoorTemperature;
        }

        //Adjust Temperature
        temp = Mathf.MoveTowards(temp, targetTemp, Time.deltaTime * adjustSpeed);

        //Update Display Temperature based on Reset Time
        updateTime += Time.deltaTime;
        if (updateTime > resetTime && (!held || equipped))
        {
            if (active)
            {
                //Inaccuracy
                float rand = Random.Range(-1f, 1f);
                displayTemp = temp + ((Mathf.Pow(rand, 3) * inaccuracyMax + rand) /2);

                //Rounding to 2 Decimal Places
                displayTemp *= 100; displayTemp = Mathf.RoundToInt(displayTemp); displayTemp /= 100;

                //Eliminates False Positives
                if (RoomManager.Instance.breakerOn && (!GameManager.ghost.GetComponent<Ghost>().FreezingTemps || !currentRoom[0].isGhostRoom))
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
            currentRoom.Insert(0,collision.GetComponent<Room>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom.Remove(collision.GetComponent<Room>());
        }
    }

    protected override void Interaction()
    {
        return;
    }
}