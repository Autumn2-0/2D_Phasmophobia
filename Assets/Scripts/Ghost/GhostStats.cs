using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will store the static values associated with each ghost type
[CreateAssetMenu(fileName = "New Ghost", menuName = "Ghost")]
[System.Serializable]
public class GhostStats : ScriptableObject
{
    public GhostType type;

    [Header("Ghost Evidence")]
    public bool FreezingTemps = false;
    public bool EMF = false;
    public bool UV = false;
    public bool Dots = false;
    public bool SpiritBox = false;
    public bool GhostOrbs = false;
    public bool GhostWriting = false;
    public bool MS = false;
    public bool Scratching = false;
    public bool Hallucinations = false;
    public bool GhostHunter9000 = false;

    [Header("Ghost Activity")]
    public int activityPerMinute = 15;
    public int huntingAPM = 30;
    [Range(0f, 10f)]
    public int toggleBreaker = 2;
    [Range(0f, 10f)]
    public int disableBreaker = 0;
    [Range(0f, 100f)]
    public int toggleLights = 25;
    [Range(0f, 100f)]
    public int turnOffLights = 25;
    [Range(0f, 100f)]
    public int breakLights = 4;
    [Range(0f, 100f)]
    public int sabotageEquipment = 30;
    [Range(0f, 200f)]
    public int throwPickup = 80;
    [Range(0f, 100f)]
    public int trackable = 10;
    [Range(0f, 100f)]
    public int hallucination = 6;
    [Range(0f, 100f)]
    public int hunt = 15;

    [Range(0f, 100f)]
    public int passiveScratching = 15;
    [Range(0f, 100f)]
    public int huntingScratching = 35;
    [Range(0f, 100f)]
    public int footprints = 60;
    [Range(0f, 100f)]
    public int dots = 30;
    [Range(0f, 100f)]
    public int write = 15;

    [Header("Evidence Info/Traits")] //Related to different evidence gathering items
    //Ghost Reach
    public float ghostReach = 5f;
    public bool reachThroughWalls = false;
    //Throw
    public float throwForceMin = 2f;
    public float throwForceMax = 4f;
    //Thermometer
    public float minRoomTemp = -4;
    public float maxRoomTemp = 0;
    //UV
    public int numberOfFootprints = 3;
    //EMF
    [Range(0f, 1f)]
    public float EMF5 = 0.2f;
    //Hunting Sanity
    public int huntingSanity;

    [Header("Roaming Movement")]
    public float roamingSpeed = 3.5f;
    [Header("Hunting Movement")]
    public float stopDistance = 1f;
    public float huntingSpeed = 7f;
    public float huntDuration = 60;
    public float huntMaxCooldown = 300;
}