using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Player player;
    public static Transform ActiveItemSlot;
    public static Ghost ghost;
    public static Vector2 mouseWorldPosition;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        ActiveItemSlot = GameObject.FindGameObjectWithTag("ItemSlot").transform;
    }

    private void Update()
    {
        mouseWorldPosition = GetMouseWorldPosition();
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }
}
