using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostOrbs : Item
{
    public static bool inUse = false;
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

    }
}
