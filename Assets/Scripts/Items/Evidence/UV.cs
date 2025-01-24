using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV : Item
{

    public float GlowRate = 0f;
    private Animator anim;
    public float range = 3f;

    public static List<Footprints> footprints;
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

        foreach (Footprints footprint in footprints)
        {
            if (StaticInteract.instance.CanReach(transform.position, footprint.transform.position, range))
            {
                footprint.visibility += GlowRate * Time.deltaTime;
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
