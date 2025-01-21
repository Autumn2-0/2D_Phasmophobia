using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f; // The range within which the mouse must be to interact
    public LayerMask interactableLayer; // The layer to filter interactable objects
    
    void Update()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPosition = GameManager.mouseWorldPosition;
        mouseWorldPosition.z = 0; // Ensure it's on the same 2D plane

        // Get the distance between the player and the mouse
        float distanceToMouse = Vector2.Distance(transform.position, mouseWorldPosition);

        // Check if the mouse is within interaction range
        if (distanceToMouse <= interactionRange)
        {
            // Cast a 2D raycast from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 0f, interactableLayer);

            if (hit.collider != null)
            {
                //Debug.Log($"Interacted with: {hit.collider.gameObject.name}");

                // Check for interaction key (0)
                if (Input.GetMouseButtonDown(0))
                {
                    HandleInteraction(hit.collider.gameObject);
                }
            }
            else
            {
                //Debug.Log("No interactable object found.");
            }
        }
        else
        {
            //Debug.Log("Mouse is out of interaction range.");
        }
    }

    void HandleInteraction(GameObject hit)
    {
        if (hit.GetComponent<Item>() != null)
            GameManager.player.Pickup(hit.GetComponent<Item>());
        else if (hit.GetComponent<Switch>() != null)
            hit.GetComponent<Switch>().Toggle();
        /**
        else if (hit.GetComponent<HidingSpot>() != null)
            GameManager.player.Hide();
        **/
    }
}
