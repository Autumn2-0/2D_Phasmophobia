using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool Breaker = false;
    public Room room;

    public void Toggle()
    {
        if (Breaker)
        {
            RoomManager.Instance.ToggleBreaker();
        }
        else
        {
            room.ToggleLights();
        }
        Debug.Log("Switch Used");
    }
}
