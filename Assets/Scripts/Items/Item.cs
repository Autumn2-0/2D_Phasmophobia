using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public abstract class Item : PickUp
{
    [SerializeField]
    protected int uses = -1;
    [SerializeField]
    protected bool active = false;
    [SerializeField]
    protected bool alwaysUpdate = false;
    
    public void Start()
    {
        base.Start();
        levelOfEMF = 3;
        StartItem();
    }

    protected virtual void StartItem() { return; }

    public void Update()
    {
        base.Update();
        if (active || alwaysUpdate)
        {
            UpdateItem();
        }
    }
    protected virtual void UpdateItem() { return; }
    
    public virtual void Use()
    {
        if (!active)
            if (uses != 0)
            {
                active = true;
                uses--;
                Interaction();
            }
            else
            {
                active = false;
            }
        else
        {
            active = false;
            Interaction();
        }
    }
}
