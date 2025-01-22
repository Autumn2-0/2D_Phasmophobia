using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    public GameObject lightBeamMain;
    public GameObject lightBeamPocket;

    protected override void UpdateItem()
    {
        if (held && !equipped)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                base.Use();
                Interaction();
            }
        }
    }
    protected override void Interaction()
    {
        if (active && (!held || equipped))
        {
            lightBeamMain.SetActive(true);
            lightBeamPocket.SetActive(false);
        }
        else if (active)
        {
            lightBeamMain.SetActive(false);
            lightBeamPocket.SetActive(true);
        }
        else
        {
            lightBeamMain.SetActive(false);
            lightBeamPocket.SetActive(false);
        }
    }
}
