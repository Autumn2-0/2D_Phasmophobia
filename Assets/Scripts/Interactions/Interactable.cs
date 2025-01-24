using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Type type;

    //Generic
    public static Breaker breaker;
    public static List<Switch> lights = new List<Switch>();
    public static List<PickUp> physicsObjects = new List<PickUp>();

    //Default Items
    public static List<Flashlight> flashlights = new List<Flashlight>();
    public static List<Thermometer> thermometers = new List<Thermometer>();
    public static List<UV> glowsticks = new List<UV>();
    public static List<EMF> emfReaders = new List<EMF>();
    public static List<SpiritBox> spiritBoxes = new List<SpiritBox>();
    public static List<DOTS> dotsProjectors = new List<DOTS>();
    public static List<GhostWriting> books = new List<GhostWriting>();
    public static List<GhostOrbs> cameras = new List<GhostOrbs>();

    //Custom Items
    public static List<Salt> salts = new List<Salt>();
    public static List<MotionSensor> motionSensors = new List<MotionSensor>();
    public static List<Candle> candles = new List<Candle>();
    public static List<SmudgeStick> smudgeSticks = new List<SmudgeStick>();
    public static List<Crucifix> crucifixs = new List<Crucifix>();
    public static List<SanityMeds> sanityMeds = new List<SanityMeds>();
    public static List<GhostHunter> ghostHunters = new List<GhostHunter>();

    protected virtual void Start()
    {
        switch (type)
        {
            case Type.Breaker:
                breaker = GetComponent<Breaker>();
                break;
            case Type.Switch:
                lights.Add(gameObject.GetComponent<Switch>());
                break;
            case Type.Physics:
                physicsObjects.Add(gameObject.GetComponent<PickUp>()); 
                break;
            case Type.Flashlight:
                physicsObjects.Add(gameObject.GetComponent<Flashlight>());
                flashlights.Add(gameObject.GetComponent<Flashlight>()); 
                break;
            case Type.Thermometer:
                physicsObjects.Add(gameObject.GetComponent<Thermometer>());
                thermometers.Add(gameObject.GetComponent<Thermometer>());
                break;
            case Type.UV:
                physicsObjects.Add(gameObject.GetComponent<UV>());
                glowsticks.Add(gameObject.GetComponent<UV>());
                break;
            case Type.EMF:
                physicsObjects.Add(gameObject.GetComponent<EMF>());
                emfReaders.Add(gameObject.GetComponent<EMF>());
                break;
            case Type.SpiritBox:
                physicsObjects.Add(gameObject.GetComponent<SpiritBox>());
                spiritBoxes.Add(gameObject.GetComponent<SpiritBox>());
                break;
            case Type.Dots:
                physicsObjects.Add(gameObject.GetComponent<DOTS>());
                dotsProjectors.Add(gameObject.GetComponent<DOTS>());
                break;
            case Type.GhostWriting:
                physicsObjects.Add(gameObject.GetComponent<GhostWriting>());
                books.Add(gameObject.GetComponent<GhostWriting>());
                break;
            case Type.Camera:
                physicsObjects.Add(gameObject.GetComponent<GhostOrbs>());
                cameras.Add(gameObject.GetComponent<GhostOrbs>());
                break;
            case Type.Salt:
                physicsObjects.Add(gameObject.GetComponent<Salt>());
                salts.Add(gameObject.GetComponent<Salt>());
                break;
            case Type.MotionSensor:
                physicsObjects.Add(gameObject.GetComponent<MotionSensor>());
                motionSensors.Add(gameObject.GetComponent<MotionSensor>());
                break;
            case Type.Candle:
                physicsObjects.Add(gameObject.GetComponent<Candle>());
                candles.Add(gameObject.GetComponent<Candle>());
                break;
            case Type.SmudgeStick:
                physicsObjects.Add(gameObject.GetComponent<SmudgeStick>());
                smudgeSticks.Add(gameObject.GetComponent<SmudgeStick>());
                break;
            case Type.Crucifix:
                physicsObjects.Add(gameObject.GetComponent<Crucifix>());
                crucifixs.Add(gameObject.GetComponent<Crucifix>());
                break;
            case Type.SanityMeds:
                physicsObjects.Add(gameObject.GetComponent<SanityMeds>());
                sanityMeds.Add(gameObject.GetComponent<SanityMeds>());
                break;
            case Type.GhostHunter:
                physicsObjects.Add(gameObject.GetComponent<GhostHunter>());
                ghostHunters.Add(gameObject.GetComponent<GhostHunter>());
                break;
        }
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
            default:
                GameManager.player.Pickup(GetComponent<PickUp>());
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
        Candle,
        SanityMeds,
        GhostWriting,
        Salt,
        Crucifix,
        SmudgeStick,
    }
}
