using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    static public RoomManager Instance;

    public Room ghostRoom;
    static public List<Room> rooms = new();

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
            rooms.Add(newRoom);
        }
        else
        {
            Debug.LogWarning("Room '" + newRoom.name + "' already exist!");
        }
    }

    public static void ToggleBreaker(bool power)
    {
        foreach (Room room in rooms)
        {
            if (power & room.lightsOn)
            {
                room.SetLightsActive(true);
            }
            if (!power)
            {
                room.SetLightsActive(false);
            }
        }
    }
}