using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWriting : Item
{
    public SpriteRenderer visuals;

    //Art
    public Color closed;
    public Color open;
    public Color written;

    //Writing Stats
    [Range(0f, 1f)]
    public static float writingChance = 0.6f;

    //Bools
    private bool writing = false;

    public override int GhostInteraction(bool itemSpecific)
    {
        if (equipped) return 0;
        if (itemSpecific)
        {
            if (GameManager.ghost.stats.GhostWriting)
            {
                if (Random.value < writingChance)
                {
                    writing = true;
                    Interaction();
                    return 4;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return base.GhostInteraction();
            }
        }
        else
        {
            if (placeable.placed)
            {
                return 0;
            }
            else
            {
                return base.GhostInteraction();
            }
        }
    }

    protected override void Interaction()
    {
        if (!placeable.placed)
        {
            visuals.color = closed;
        }
        else if (writing)
        {
            visuals.color = written;
        }
        else
        {
            visuals.color = open;
        }
    }
}
