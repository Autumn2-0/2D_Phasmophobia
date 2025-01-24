using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : Interactable
{
    public static Breaker Instance;

    private static Breaker[] allInstances;

    void Awake()
    {
        allInstances = FindObjectsOfType<Breaker>();
        if (Instance == null)
        {
            Instance = allInstances[Random.Range(0, allInstances.Length)];
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (Instance == null)
        {
            Instance = allInstances[Random.Range(0, allInstances.Length)];
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void Toggle()
    {
        RoomManager.Instance.ToggleBreaker();
    }
    public static bool isBreakerOn()
    {
        return RoomManager.Instance.breakerOn;
    }

    public void GhostInteraction(bool toggle)
    {
        if (toggle)
        {
            Toggle();
            gameObject.AddComponent<Interaction>().Initiate(3);
        }
        else
        {
            if (isBreakerOn())
            {
                Toggle();
                gameObject.AddComponent<Interaction>().Initiate(3);
            }
        }
    }
}
