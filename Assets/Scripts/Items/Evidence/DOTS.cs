using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTS : Item
{
    public GameObject projection;
    public float range = 5f;
    public bool requiresPlaced = true;
    public static bool running = false;
    public float activeVisibility = 0.25f;
    
    protected override void StartItem()
    {
        if (running == false)
        {
            StartCoroutine(Projectors());
        }
    }

    protected override void UpdateItem()
    {
        
    }

    public static IEnumerator Projectors()
    {
        while (true)
        {
            foreach (DOTS projector in Interactable.dotsProjectors)
            {
                if (projector.active && (projector.placeable.placed || (!projector.requiresPlaced && (!projector.equipped || projector.inHand))))
                {
                    if (StaticInteract.instance.CanReach(projector.transform.position, GameManager.ghost.transform.position, projector.range))
                    {
                        GameManager.ghost.SetDotsVisibility(projector.activeVisibility);
                        yield return null;
                    }
                }
            }
            GameManager.ghost.SetDotsVisibility(0f); yield return null;
        }
    }

    public override int GhostInteraction(bool itemSpecific)
    {
        return base.GhostInteraction();
    }

    protected override void Interaction()
    {
        projection.SetActive(active && (placeable.placed || (!requiresPlaced && (!equipped || inHand))));
    }
}
