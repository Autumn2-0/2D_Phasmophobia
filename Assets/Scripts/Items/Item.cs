using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public abstract class Item : PickUp
{
    public int uses = -1;
    [SerializeField]
    protected bool active = false;
    [SerializeField]
    protected bool alwaysUpdate = false;
    public bool electronic = false;
    
    protected override void Start()
    {
        base.Start();
        if (electronic)
        {
            Interactable.electronics.Add(this);
        }
        StartItem();
    }

    protected virtual void StartItem() { return; }

    protected override void Update()
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

    public bool GetActive()
    { return active; }
}
