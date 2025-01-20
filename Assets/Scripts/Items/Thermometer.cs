using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : Item
{
    public float inaccuracyMax = 4f;
    public float temp = 20;
    public float targetTemp = 20;
    float updateTime = 0;
    public float resetTime;
    public float adjustSpeed = 0.075f;

    protected override void UpdateItem()
    {
        temp = Mathf.MoveTowards(temp, targetTemp, Time.deltaTime * adjustSpeed);
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
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            targetTemp = collision.GetComponent<Room>().temperature;
        }
    }
}
