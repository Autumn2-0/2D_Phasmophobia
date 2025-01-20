using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : Item
{
    public float inaccuracyMax = 0.25f;
    float value = 20;
    float targetTemp = 20;

    protected override void ItemStart()
    {
        pocketable = true;
    }
    protected override void UpdateItem()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            targetTemp = collision.GetComponent<Room>().temperature;
        }
    }
}
