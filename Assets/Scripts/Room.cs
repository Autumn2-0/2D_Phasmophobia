using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public int temperature = 65;

    public void ChangeTemperature(int amount)
    {
        temperature += amount;

        temperature = Mathf.Clamp(temperature, 15, 65);
    }
}