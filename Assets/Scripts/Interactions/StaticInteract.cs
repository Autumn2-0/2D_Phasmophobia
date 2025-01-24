using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StaticInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public LayerMask interactableLayer;
    public LayerMask wallLayer;
    [HideInInspector]
    public static StaticInteract instance;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public bool CanReach(Vector2 startPos, Vector2 endPos, float range, bool ignoreWalls = false)
    {
        float distance = Vector2.Distance(startPos, endPos);
        if (distance > range)
        {
            return false;
        }
        RaycastHit2D ray = Physics2D.Raycast(startPos, endPos - startPos, distance, wallLayer);
        if (ray.collider != null && !ignoreWalls)
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
