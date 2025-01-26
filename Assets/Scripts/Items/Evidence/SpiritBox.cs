using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBox : Item
{
    /**
     * Due to not having Voice Input or a multiple choice question system implemented, spirit box will work different for now
     * When activated it will turn itself back off and set a timer until it can be activated again. We can have a sound run during that time
     * Each activation acts as a question for the ghost which then has a chance to respond if in range.
     **/

    public float resetTime = 1f;
    public bool reset = true;
    public float range = 4f;
    
    protected override void StartItem()
    {

    }

    protected override void UpdateItem()
    {

    }

    public override int GhostInteraction(bool itemSpecific)
    {
        if (active)
        {
            TalkToGhost();
            return 1;
        }
        else
            return base.GhostInteraction();
    }

    protected override void Interaction()
    {
        if (active)
        {
            if (reset)
            {
                StartCoroutine(GhostResponse());
            }
        }
    }

    private bool TalkToGhost()
    {
        bool canReach = StaticInteract.instance.CanReach(transform.position, GameManager.ghost.transform.position, range * GameManager.ghost.stats.spiritBoxRangeMultiplier);
        bool lightsOff = !GameManager.player.currentRoom.lightsOn;
        if (lightsOff && canReach && Random.Range(0f,1f) < GameManager.ghost.stats.responseChance && GameManager.ghost.stats.SpiritBox)
        {
            Debug.Log("Ghost Responded");
            return true;
        }
        else
        {
            Debug.Log("No Response");
            return false;
        }
    }

    public IEnumerator GhostResponse()
    {
        reset = false;
        TalkToGhost();
        yield return new WaitForSeconds(resetTime);
        reset = true;
        active = false;
    }
}
