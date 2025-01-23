using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Type type;

    //Generic
    public static List<Breaker> breaker;
    public static List<Switch> lights;
    public static List<PickUp> physicsObjects;

    //Default Items
    public static List<Flashlight> flashlight;
    public static List<Thermometer> thermometers;
    public static List<UV> glowsticks;
    public static List<EMF> emfReaders;
    public static List<SpiritBox> spiritBoxes;
    public static List<DOTS> dotsProjectors;
    public static List<GhostWriting> books;
    public static List<Camera> cameras;

    //Custom Items
    public static List<Salt> salts;
    public static List<MotionSensor> motionSensors;
    public static List<Candle> candles;
    public static List<SmudgeStick> smudgeAticks;
    public static List<Crucifix> crucifixs;
    public static List<Scratching> scratches;
    public static List<GhostHunter> ghostHunters;

    void Start()
    {
        switch (type)
        {
            case Type.Breaker:
                breaker.Add(this.GetComponent<Breaker>());
                break;
            case Type.Switch:
                lights.Add(this.GetComponent<Switch>());
                break;
            case Type.Physics:
                physicsObjects.Add(this.GetComponent<PickUp>()); 
                break;
            case Type.GhostWriting:
                physicsObjects.Add(this.GetComponent<PickUp>());
                books.Add(this.GetComponent<GhostWriting>()); 
                break;
        }
    }

    public void GhostInteraction()
    {

    }

    public void Interact()
    {
        switch (type)
        {
            case Type.Breaker:
                Breaker.Toggle();
                break;
            case Type.Switch:
                GetComponent<Switch>().Toggle();
                break;
            case Type.Physics:
            case Type.GhostWriting:
                GameManager.player.Pickup(GetComponent<PickUp>().GhostInteraction);
                break;
        }
    }

    public enum Type
    {
        Breaker,
        Switch,
        Physics,
        Flashlight,
        Thermometer,
        UV,
        GhostHunter,
        EMF,
        Dots,
        Camera,
        SpiritBox,
        MotionSensor,
        Scratching,
        Candle,
        SanityMeds,
        GhostWriting,
        Salt,
        Crucifix,
        SmudgeStick,
    }
}
