using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()   
    {
        // Get input from keyboard (horizontal and vertical axis)
        movement.x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        movement.y = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        playerModel.transform.up = GameManager.mouseWorldPosition - transform.position;
        cam.transform.position += (playerModel.transform.position + (Vector3)movement.normalized * camRange - cam.transform.position) * camSpeed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
