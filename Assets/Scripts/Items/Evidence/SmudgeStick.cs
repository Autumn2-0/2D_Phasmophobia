using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmudgeStick : Item
{
    public SpriteRenderer visuals;

    //Art
    public Color unused;
    public Color burning;
    public Color burnt;

    //Writing Stats
    [Range(0f, 5f)]
    public float smudgeDuration = 4f;
    public float smudgeRange = 3f;
    public float burningDuration = 1f;

    //Bools
    private bool inUse = false;
    private bool hitGhost = false;

    protected override void StartItem()
    {
        visuals.color = unused;
    }

    protected override void UpdateItem()
    {
        if (!inUse)
        {
            inUse = true;
            StartCoroutine(Smudging());
        }

        if (!hitGhost)
        {
            if (StaticInteract.instance.CanReach(transform.position, GameManager.ghost.transform.position, smudgeRange))
            {
                GameManager.ghost.Smudge(smudgeDuration);
                hitGhost = true;
            }
        }
    }

    private IEnumerator Smudging()
    {
        visuals.color = burning;
        yield return new WaitForSeconds(burningDuration);
        visuals.color = burnt;
        active = false;
    }

    public override int GhostInteraction(bool itemSpecific)
    {
        return base.GhostInteraction();
    }

    protected override void Interaction()
    {

    }
}
