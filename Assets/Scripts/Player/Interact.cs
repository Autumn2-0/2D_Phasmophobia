using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Interact : MonoBehaviour
{
    [Header("Interaction Settings")]
    public LayerMask interactableLayer = LayerMask.GetMask("Walls");
    public LayerMask wallLayer = LayerMask.GetMask("Interactable");
    [HideInInspector]
    public static Interact instance;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public bool CanReach(Vector2 startPos, Vector2 endPos, float range)
    {
        float distance = Vector2.Distance(startPos, endPos);
        if (distance > range)
        {
            return false;
        }
        RaycastHit2D ray = Physics2D.Raycast(startPos, endPos - startPos, distance, wallLayer);
        if (ray.collider != null)
        {
            return false;
        }
        return true;
    }

    public void Interaction(Vector2 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, interactableLayer);

        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<Interactable>().Interact();
        }
    }
}
