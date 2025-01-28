using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMF : Item
{
    public float detectionRange = 4f;
    public bool detectsThroughWalls = false;
    protected override void StartItem()
    {

    }

    protected override void UpdateItem()
    {
        if (active)
        {
            int EMF = 1;
            for (int i = 0; i < InteractionMarking.Interactions.Count; i++)
            {
                if (InteractionMarking.Interactions[i] == null)
                {
                    InteractionMarking.Interactions.Remove(InteractionMarking.Interactions[i]);
                    i--;
                    continue;
                }
                if (StaticInteract.instance.CanReach(transform.position, InteractionMarking.Interactions[i].transform.position, detectionRange, detectsThroughWalls))
                {
                    EMF = InteractionMarking.Interactions[i].EMF;
                    break;
                }
            }
            OutputEMF(EMF);
        }
    }

    private void OutputEMF(int EMF)
    {
        if (inHand) Debug.Log(EMF);
    }

    public override int GhostInteraction(bool itemSpecific)
    {
        return base.GhostInteraction();
    }

    protected override void Interaction()
    {

    }
}
