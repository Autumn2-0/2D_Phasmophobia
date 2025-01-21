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
        temp = Mathf.MoveTowards(temp, currentRoom.temperature, Time.deltaTime * adjustSpeed);
        displayTemp = temp + Mathf.Sqrt(Random.Range(0, inaccuracyMax * inaccuracyMax));
        displayTemp *= 100; displayTemp = Mathf.RoundToInt(displayTemp); displayTemp /= 100; 
        updateTime += Time.deltaTime;
        if (updateTime > resetTime && (!held || equipped))
        {
            while (displayTemp <= 1 && (!GameManager.ghost.GetComponent<Ghost>().FreezingTemps))
                displayTemp += Random.Range(0.01f, 2f);
            if (active)
                Debug.Log(displayTemp);
            updateTime -= resetTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom = collision.GetComponent<Room>();
        }
    }
}