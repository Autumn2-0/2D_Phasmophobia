using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHunter : Item
{
    float dotProduct;
    float distance;
    public float minDot = 0.25f;
    public float maxDistance = 5;
    public float minReading = 0.7f;
    int currentReading;
    protected override void UpdateItem()
    {
        dotProduct = Vector2.Dot(transform.up, (GameManager.ghost.transform.position - transform.position).normalized);
        distance = (GameManager.ghost.transform.position - transform.position).magnitude;

        currentReading = 0;
        if (dotProduct > minDot && StaticInteract.instance.CanReach(transform.position, GameManager.ghost.transform.position, maxDistance) && dotProduct * (1 - distance / maxDistance) > minReading)
        {
            currentReading = 1;
            if (GameManager.ghost.activeGhostHunter)
            {
                currentReading = 2;
                if (GameManager.ghost.stats.huntsWhenTracked) GameManager.ghost.TriggerEarlyHunt();
            }
        }
        if (inHand) Debug.Log(currentReading);
    }

    protected override void Interaction()
    {
        return;
    }
}
