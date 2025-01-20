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
    public Item[] items = new Item[3];
    public int currentSlot = 0;

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
        if (equippedItem != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                equippedItem.Drop();
                items[currentSlot] = null;
                return;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                equippedItem.Place();
                items[currentSlot] = null;
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                equippedItem.Use();
            }
            if (Input.GetKeyDown("1"))
            {
                currentSlot = 0;
                equippedItem.PocketItem();
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
            if (Input.GetKeyDown("2"))
            {
                currentSlot = 1;
                equippedItem.PocketItem();
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
            if (Input.GetKeyDown("3"))
            {
                currentSlot = 2;
                equippedItem.PocketItem();
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
        }
        else
        {
            if (Input.GetKeyDown("1"))
            {
                currentSlot = 0;
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
            if (Input.GetKeyDown("2"))
            {
                currentSlot = 1;
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
            if (Input.GetKeyDown("3"))
            {
                currentSlot = 2;
                if (items[currentSlot] != null)
                    items[currentSlot].EquipItem();
            }
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
            items[currentSlot] = item;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (items[i] == null)
                {
                    item.PocketItem();
                    items[i] = item;
                    return;
                }
            }
        }
    }

}
