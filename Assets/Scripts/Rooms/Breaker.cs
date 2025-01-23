using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    public static Breaker Instance;

    private static Breaker[] allInstances;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectsOfType<Breaker>()[Random.Range(0, allInstances.Length)];
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void Toggle()
    {
        RoomManager.Instance.ToggleBreaker();
        Debug.Log("Breaker Toggled");
    }
    public static bool isBreakerOn()
    {
        return RoomManager.Instance.breakerOn;
    }
}
