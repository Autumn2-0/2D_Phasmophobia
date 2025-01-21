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

    public float buildingTemperature = 20f; //celcius
    public float outdoorTemperature = 0f; //celcius

    public static float temperatureRecalculateTime = 30f;
    public static float temperatureVariation = 4f;

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

    public void Start() //Initialize Rooms
    {
        breakerOn = !breakerStartsOn;
        ToggleBreaker();
        buildingTemperature = Random.Range(18f, 25f);
        outdoorTemperature = Random.Range(-8f, 12f);
        
        for (int i = 0; i < transform.childCount; ++i)
        {
            rooms.Add(transform.GetChild(i).GetComponent<Room>());
            if (ghostRoom == null && Random.Range(0, transform.childCount - i) == 0)
            {
                ghostRoom = rooms[i];
                ghostRoom.isGhostRoom = true;
                GameManager.ghost.gameObject.transform.position = ghostRoom.ghostSpawnLocation.position;
            }

            //Initializing Temperatures
            if (breakerOn)
            {
                if (ghostRoom == rooms[i])
                {
                    rooms[i].SetStartingTemperature(Random.Range(GameManager.ghost.stats.minRoomTemp, GameManager.ghost.stats.maxRoomTemp));
                }
                else
                {
                    rooms[i].SetStartingTemperature(buildingTemperature + Random.Range(-temperatureVariation, temperatureVariation));
                }
            }
            else
            {
                rooms[i].SetStartingTemperature(outdoorTemperature + Random.Range(-temperatureVariation, temperatureVariation));
            }
        }

        InvokeRepeating("UpdateTemperatures", temperatureRecalculateTime, temperatureRecalculateTime);
    }

    public void ToggleBreaker()
    {
        breakerOn = !breakerOn;
        foreach (Room room in rooms)
        {
            room.BreakerUpdate();
        }
        RecalculatePower();
        UpdateTemperatures();
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

    public void UpdateTemperatures()
    {
        foreach (Room room in rooms)
        {
            //Initializing Temperatures
            if (breakerOn)
            {
                if (ghostRoom == room)
                {
                    room.SetTargetTemperature(Random.Range(GameManager.ghost.stats.minRoomTemp, GameManager.ghost.stats.maxRoomTemp));
                }
                else
                {
                    room.SetTargetTemperature(buildingTemperature);
                }
            }
            else
            {
                room.SetTargetTemperature(outdoorTemperature);
            }
        }
    }

    public void ChangeGhostRoom(Room room)
    {
        ghostRoom.isGhostRoom = false;
        ghostRoom = room;
        ghostRoom.isGhostRoom = true;
        UpdateTemperatures();
    }

}