using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
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

    public void GhostInteraction(int EMF)
    {
        InteractionMarking.Instantiate(gameObject, EMF);
    }
}
