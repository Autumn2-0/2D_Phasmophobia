using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will store the static values associated with each ghost type
[CreateAssetMenu(fileName = "New Ghost", menuName = "Ghost")]
[System.Serializable]
public class GhostStats : ScriptableObject
{
    public GhostType type;

    [Header("Area")]
    public int temperatureModifier = -4;

    [Header("Evidence Info/Traits")] //Related to different evidence gathering items
    //Thermometer
    public float minRoomTemp = -4;
    public float maxRoomTemp = 0;
    //GhostWriting
    public float averageWritingTime = 18f;
    public float variationWritingTime = 13f;

    [Header("Roaming Movement")]
    public float roamingSpeed = 3.5f;
    [Header("Hunting Movement")]
    public float stopDistance = 1f;
    public float huntingSpeed = 7f;
    public float huntDuration = 60;
    public float huntMaxCooldown = 300;
}