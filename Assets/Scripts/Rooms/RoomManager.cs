using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    static public RoomManager Instance;

    public Room ghostRoom;
    static public List<Room> rooms = new();

    bool breakerOn = false;
    float powerUsage = 0f;
    float maxPower = 100f;

    public float buildingTemperature = 12f; //celcius

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void InstantiateRoom(Room newRoom)
    {
        if (!rooms.Contains(newRoom))
        {
            newRoom.SetTemperature(buildingTemperature);
            newRoom.BreakerUpdate(breakerOn);
            rooms.Add(newRoom);
        }
        else
        {
            Debug.LogWarning("Room '" + newRoom.name + "' already exist!");
        }
    }

    public void ToggleBreaker()
    {
        foreach (Room room in rooms)
        {
            room.BreakerUpdate(breakerOn);
        }
        RecalculatePower();
    }

    public void RecalculatePower()
    {
        powerUsage = 0;
        foreach(Room room in rooms)
        {
            powerUsage += room.GetCurrentPower();
        }
        if (powerUsage > maxPower)
        {
            foreach (Room room in rooms)
            {
                room.BreakerFailure();
            }
        }
    }
}