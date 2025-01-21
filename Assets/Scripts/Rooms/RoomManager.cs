using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    static public RoomManager Instance;

    public Room ghostRoom;
    static public List<Room> rooms = new();

    public bool breakerOn = false;
    public bool breakerStartsOn = false;

    public float powerUsage = 0f;
    public float maxPower = 100f;

    public float buildingTemperature = 12f; //celcius
    public float outdoorTemperature = 8f; //celcius

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

    public void Start()
    {
        breakerOn = !breakerStartsOn;
        ToggleBreaker();
    }

    public void InstantiateRoom(Room newRoom)
    {
        if (!rooms.Contains(newRoom))
        {
            newRoom.SetTemperature(buildingTemperature);
            newRoom.BreakerUpdate();
            rooms.Add(newRoom);
        }
        else
        {
            Debug.LogWarning("Room '" + newRoom.name + "' already exist!");
        }
    }

    public void ToggleBreaker()
    {
        breakerOn = !breakerOn;
        foreach (Room room in rooms)
        {
            room.BreakerUpdate();
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
            breakerOn = false;
            foreach (Room room in rooms)
            {
                room.BreakerFailure();
            }
        }
    }
}