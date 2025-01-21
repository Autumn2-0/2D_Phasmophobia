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

    /**
     * Each Ghost Type should have a range of possible temperatures for their ghost room, for example
     * a Hantu will have the ghost room temperature range from -10 to -5 celcius. The temperature will
     * fluctuate between that value. Additionally the temperature of each room including the ghost room
     * should approach anywhere from 0-5 celcius when the breaker is on. I don't want to adjust the
     * code here since I don't know how it works however if you could implement this that would be great.
     * -Andy
     **/

    public float minRoomTemp = -4; //Added this for room manager - Ghost Room Temp
    public float maxRoomTemp = 0; //Added this for room manager - Ghost Room Temp

    [Header("Roaming Movement")]
    public float roamingSpeed = 3.5f;
    [Header("Hunting Movement")]
    public float stopDistance = 1f;
    public float huntingSpeed = 7f;
    public float huntDuration = 60;
    public float huntMaxCooldown = 300;
}