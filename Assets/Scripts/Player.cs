using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player movement

    private Rigidbody2D rb;
    private Vector2 movement;

    public GameObject cam;
    public GameObject playerModel;
    public float camRange;
    public float camSpeed;
    private Vector2 movementInput;
    private Item[] items = new Item[3];
    private int currentSlot = 0;

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        movement.y = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow
        playerModel.transform.up = GameManager.mouseWorldPosition - transform.position;
        cam.transform.localPosition = Vector2.Lerp((Vector2)cam.transform.localPosition, movement.normalized * camRange, camSpeed * Time.deltaTime);

        Item equippedItem = items[currentSlot];
        //Player Inputs
        if (Input.GetKeyDown(KeyCode.Q))
        {
            equippedItem.Drop();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            equippedItem.Place();
        }
        if (Input.GetMouseButtonDown(1))
        {
            equippedItem.Use();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentSlot = 0;
            equippedItem.PocketItem();
            items[currentSlot].EquipItem();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentSlot = 1;
            equippedItem.PocketItem();
            items[currentSlot].EquipItem();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentSlot = 2;
            equippedItem.PocketItem();
            items[currentSlot].EquipItem();
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
        //rb.velocity = movementInput * moveSpeed;  New Inputs weren't working. Using old system so I can test other things
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    public void Pickup(Item item)
    {
        if (items[currentSlot] == null)
        {
            item.EquipItem();
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (items[i] == null)
                {
                    item.PocketItem();
                }
            }
        }
    }

}
