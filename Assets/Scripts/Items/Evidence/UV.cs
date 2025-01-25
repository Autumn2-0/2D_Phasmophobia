using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV : Item
{

    public float GlowRate = 0f;
    public float GlowMod = 0.4f;
    private Animator anim;
    public float range = 3f;

    public static List<Footprints> footprints = new List<Footprints>();
    protected override void StartItem()
    {
        anim = GetComponent<Animator>();
    }

    protected override void UpdateItem()
    {
        if (equipped && !inHand)
        {
            gameObject.SetActive(false);
        }
        anim.SetBool("IsActive", active);

        if (active)
        {
            foreach (Footprints footprint in footprints)
            {
                bool canReach = StaticInteract.instance.CanReach(transform.position, footprint.transform.position, range);
                if (canReach)
                {
                    footprint.visibility += GlowRate * GlowMod * Time.deltaTime;
                }
            }
        }
    }

    protected override void Interaction()
    {
        if (!equipped || inHand)
        {
            gameObject.SetActive(true);
        }
    }
}
