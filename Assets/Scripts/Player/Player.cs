using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public PickUp[] items = new PickUp[3];
    public int currentSlot = 0;

    public float playerReach = 2.5f;
    public float throwForce = 2f;

    private List<Room> currentRooms = new List<Room>();
    public Room currentRoom;
    public bool detectableByElectronics = false;

    private void Awake()
    {
        GameManager.player = this;
    }

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        movement.y = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow
        playerModel.transform.up = GameManager.mouseWorldPosition - (Vector2)transform.position;
        cam.transform.localPosition = Vector2.Lerp((Vector2)cam.transform.localPosition, movement.normalized * camRange, camSpeed * Time.deltaTime);

        PickUp equippedItem = items[currentSlot];
        //Player Inputs
        if (equippedItem != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                equippedItem.Throw(throwForce, true);
                items[currentSlot] = null;
                return;
            }
            if (Input.GetKeyDown(KeyCode.F) && equippedItem.GetComponent<Placeable>())
            {
                if (equippedItem.GetComponent<Placeable>() != null && equippedItem.GetComponent<Placeable>().Place())
                    items[currentSlot] = null;
                return;
            }
            if (Input.GetMouseButtonDown(1) && equippedItem.GetComponent<Item>() != null)
            {
                equippedItem.GetComponent<Item>().Use();
            }
        }
        if (Input.GetMouseButtonDown(0) && StaticInteract.instance.CanReach(transform.position, GameManager.mouseWorldPosition, playerReach))
        {
            StaticInteract.instance.Interaction(GameManager.mouseWorldPosition);
        }
        if (Input.GetKeyDown("1"))
        {
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(false);
            currentSlot = 0;
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(true);
        }
        if (Input.GetKeyDown("2"))
        {
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(false);
            currentSlot = 1;
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(true);
        }
        if (Input.GetKeyDown("3"))
        {
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(false);
            currentSlot = 2;
            if (items[currentSlot] != null)
                items[currentSlot].EquipItem(true);
        }

        detectableByElectronics = false;
        foreach (var item in items)
        {
            Item current = item.GetComponent<Item>();
            if (current != null)
            {
                if (current.electronic && current.GetActive())
                    detectableByElectronics = true;
            }
        }
        
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }

    public void Pickup(PickUp item)
    {
        if (items[currentSlot] == null)
        {
            item.EquipItem(true);
            items[currentSlot] = item;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (items[i] == null)
                {
                    item.EquipItem(false);
                    items[i] = item;
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Insert(0, collision.GetComponent<Room>());
            currentRoom = currentRooms[0];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRooms.Remove(collision.GetComponent<Room>());
            if (currentRooms.Count() > 0)
                currentRoom = currentRooms[0];
            else currentRoom = null;
        }
    }
}
