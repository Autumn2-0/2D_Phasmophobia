using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crucifix : Item
{
    public float range = 5f;
    public bool throughWalls = false;
    protected override void StartItem()
    {

    }

    protected override void UpdateItem()
    {

    }

    public override int GhostInteraction(bool itemSpecific)
    {
        return base.GhostInteraction();
    }

    protected override void Interaction()
    {
        if (active)
        {
            active = false;
            uses++;
        }
    }
}
