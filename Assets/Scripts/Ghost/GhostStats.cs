using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will store the static values associated with each ghost type
[CreateAssetMenu(fileName = "New Ghost", menuName = "Ghost")]
[System.Serializable]
public class GhostStats : ScriptableObject
{
    public GhostType type;

    //Settings - Freezing Temps
    [Header("Settings - Freezing Temperature")]
    public bool FreezingTemps = false;
    [ShowIf("FreezingTemps")]
    public float freezingRoomTemp = -2;

    //Settings - EMF 5
    [Header("Settings - EMF 5")]
    public bool EMF = false;
    [ShowIf("EMF"), Range(0f, 1f)]
    public float chanceOfEMF = 0.2f;

    //Settings - UV
    [Header("Settings - UV")]
    public bool UV = false;
    [ShowIf("UV"), Range(0f, 100f)]
    public int footprints = 60;

    //Settings - D.O.T.S Projector
    [Header("Settings - D.O.T.S Projector")]
    public bool Dots = false;
    [ShowIf("Dots"), Range(0f, 100f)]
    public int dots = 30;
    [ShowIf("Dots")]
    public int dotsLength = 8;

    //Settings - Spirit Box
    [Header("Settings - Spirit Box")]
    public bool SpiritBox = false;
    [ShowIf("SpiritBox"), Range(0f, 1f)]
    public float responseChance = 0.25f;
    [ShowIf("SpiritBox")]
    public float spiritBoxRangeMultiplier = 1;

    //Settings - Ghost Orbs
    [Header("Settings - Ghost Orbs")]
    public bool GhostOrbs = false;
    [ShowIf("GhostOrbs")]
    public float orbsSpawnMin = 0.25f;
    [ShowIf("GhostOrbs")]
    public float orbsSpawnMax = 10f;

    //Settings - Ghost Writing
    [Header("Settings - Ghost Writing")]
    public bool GhostWriting = false;
    [ShowIf("GhostWriting"), Range(0f, 100f)]
    public int write = 15;

    //Settings - Motion Sensor
    [Header("Settings - Motion Sensor")]
    public bool MotionSensor = false;

    //Settings - Salt
    [Header("Settings - Salt")]
    public bool Salt = false;

    //Settings - Scratching
    [Header("Settings - Scratching")]
    public bool Scratching = false;
    [ShowIf("Scratching")]
    public bool highSanityScratching = false;
    [ShowIf("Scratching"), Range(0f, 100f)]
    public int passiveScratching = 15;
    [ShowIf("Scratching"), Range(0f, 100f)]
    public int huntingScratching = 35;

    //Settings - Hallucinations
    [Header("Settings - Hallucinations")]
    public bool Hallucinations = false;

    //Settings - Ghost Hunter 9000
    [Header("Settings - Ghost Hunter 9000")]
    public bool GhostHunter9000 = false;
    [ShowIf("GhostHunter9000"), Range(0f, 100f)]
    public int trackable = 10;

    //Settings - Smudge Stick
    [Header("Settings - Smudge Stick")]
    public float smudgeTimer = 30;
    public float smudgeSpeed = 30;

    //Settings - Ghost Activity
    [Header("Settings - Ghost Activity")]
    public int activityPerMinute = 15;
    [ShowIf("customActivityNearPlayer")]
    public int activityNearPlayer = 15;
    public int huntingAPM = 30;
    [Range(0f, 10f)]
    public int toggleBreaker = 2;
    [Range(0f, 10f)]
    public int toggleLights = 25;
    [Range(0f, 100f)]
    public int breakLights = 4;
    [Range(0f, 100f)]
    public int eventLights = 3;
    [Range(0f, 100f)]
    public int sabotageEquipment = 30;
    [Range(0f, 200f)]
    public int throwPickup = 80;
    [Range(0f, 100f)]
    public int hallucination = 6;
    [Range(0f, 100f)]
    public int hunt = 15;

    //Settings - Ghost Traits
    [Header("Settings - Ghost Traits")] //Related to different evidence gathering items
    //Ghost Reach
    public float ghostReach = 5f;
    //Throw
    public float throwForceMin = 2f;
    public float throwForceMax = 4f;
    //Hunting
    public int huntingSanity = 70;
    [Range(0f, 1f)]
    public float blinkMinRate = 0.2f;
    [Range(0f, 1f)]
    public float blinkMaxRate = 0.4f;

    [Header("Settings - Unique Traits")]
    [ShowIf("UV")]
    public bool modifyNumberOfFootprits = false; //Modify Number of Footprints
    [ShowIf("modifyNumberOfFootprits"), Range(-2,2)]
    public int numberOfFootprints = 3;
    public bool adjustVisibility = false; //Phantom & Oni
    [ShowIf("adjustVisibility"), Range(0f, 1f)]
    public float huntingVisibleChance = 0.5f;
    [ShowIf("adjustVisibility"), Range(0f, 1f)]
    public float visibilityToggleChance = 0.5f;
    public bool huntingModelSwap = false; //Obake
    [ShowIf("Dots")]
    public bool dotsRequireCamera = false; //Goryo
    public bool alwaysTracksPlayer = false; //Deogen
    public bool speedRelativeToDistance = false;
    public bool ghostModifiedDetectionRange = false; //Yokai
    [ShowIf("ghostModifiedDetectionRange"), Range(0f,50f)]
    public float electronicsDetectionRange = 15f;
    public bool electronicsBoostSpeed = false; //Raiju
    [ShowIf("electronicsBoostSpeed")]
    public float electronicsSpeedBoost = 1f;
    [ShowIf("electronicsBoostSpeed")]
    public float electronicsBoostRange = 4;
    public bool onlyTurnLightsOff = false; //Mare
    public bool onlyTurnBreakerOff = false; //Jinn
    public bool huntStartsOnPlayer = false; //Banshee
    [ShowIf("FreezingTemps")]
    public bool temperaturesBoostSpeed = false; //Hantu
    [ShowIf("temperaturesBoostSpeed")]
    public float temperatureSpeedBoost = 1f;
    [ShowIf("temperaturesBoostSpeed")]
    public float temperatureBoostThreshold = 4f;
    public bool customActivityNearPlayer = false; //Shade & Moroi
    [ShowIf("FreezingTemps")]
    public bool currentRoomFreezes = false; //New Ghost
    [ShowIf("GhostHunter9000")]
    public bool huntsWhenTracked = false; //New Ghost
    public bool reachThroughWalls = false; //New Ghost
    public bool additionalSanityDrain = false; //New Ghosts
    [ShowIf("GhostHunter9000")]
    public float sanityDrainModifier = 1;
    [ShowIf("GhostHunter9000"), Range(0f,1f)]
    public float roomSanityDrain = 0f;
    [ShowIf("GhostHunter9000"), Range(0f, 1f)]
    public float proximitySanityDrain = 0f;

    //Settings - Roaming
    [Header("Settings - Roaming")]
    public float roamingSpeed = 3.5f;
    public float roamingRange = 3.5f;
    public float returnToRoom = 0.4f;
    public float replaceRoom = 0.2f;
    
    //Settings - Hunting
    [Header("Settings - Hunting")]
    public float stopDistance = 1f;
    public float huntingSpeed = 7f;
    public float huntingMaxSpeed = 8f;
    public float huntSpeedUpTime = 3f;
    public float huntDuration = 60; //Length of hunt
    public float gracePeriod = 300; //How long until first hunt
    public float preHuntTimer = 4;
    public float huntTimer = 60; //Time between hunts
    public float ghostVisionRange = 30; //How far until ghost can see you in a hunt
    public float trackingDuration = 2.5f; //How long after being seen until ghost forgets your location

    
}