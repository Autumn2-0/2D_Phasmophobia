using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : Item
{
    public Room currentRoom;

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
        if (temp < GameManager.player.currentRoom.temperature)
        {
            temp += adjustSpeed * Time.deltaTime;
            temp = (Mathf.RoundToInt(temp * 100)) / 100;
            if (temp > currentRoom.temperature)
            {
                temp = currentRoom.temperature;
            }
        }
        else if (temp > currentRoom.temperature)
        {
            temp -= adjustSpeed * Time.deltaTime;
            temp = (Mathf.RoundToInt(temp * 100)) / 100;
            if (temp < currentRoom.temperature)
            {
                temp = currentRoom.temperature;
            }
        }
        displayTemp = temp + Random.Range(-inaccuracyMax, inaccuracyMax);

        /*temp = Mathf.MoveTowards(temp, targetTemp, Time.deltaTime * adjustSpeed);
        float value = temp + Mathf.Sqrt(Random.Range(0, inaccuracyMax * inaccuracyMax));
        value *= 100; value = Mathf.RoundToInt(value); value /= 100; 
        updateTime += Time.deltaTime;
        if (updateTime > resetTime && (!held || equipped))
        {
            while (value <= 1 && (!GameManager.ghost.GetComponent<Ghost>().FreezingTemps))
                value += Random.Range(0.01f, 2f);
            if (active)
                Debug.Log(value);
            updateTime -= resetTime;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom = collision.GetComponent<Room>();
        }
    }
}