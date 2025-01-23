using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Room room;

    public void Toggle()
    {
        room.ToggleLights();
        Debug.Log("Switch Used");
    }

    public bool isLightsOn()
    {
        return room.lightsOn;
    }
}
