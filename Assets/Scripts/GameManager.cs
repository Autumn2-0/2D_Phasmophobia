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
    public static Vector3 mouseWorldPosition;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
        ActiveItemSlot = GameObject.FindGameObjectWithTag("ItemSlot").transform;
    }

    private void Update()
    {
        mouseWorldPosition = GetMouseWorldPosition();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0; // Ensure z is set correctly for 2D
        return mouseWorldPosition;
    }
}
