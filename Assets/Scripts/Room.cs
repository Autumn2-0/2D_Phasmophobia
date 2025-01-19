using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will controll all the information and behaviour related to individual rooms.

public class Room : MonoBehaviour
{
    public int EMF = 1;
    public int temperature = 65;

    public GameObject lights;

    //Add to the EMF value for the room
    public void ChangeEMF(int amount)
    {
        EMF += amount;

        EMF = Mathf.Clamp(EMF, 1, 5);
    }

    //Add to the Temperature value for the room
    public void ChangeTemperature(int amount)
    {
        temperature += amount;

        temperature = Mathf.Clamp(temperature, 15, 65);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lights.SetActive(true);
        }
        else if (collision.CompareTag("Ghost"))
        {
            //Add the logic for changing the temperature and EMF based on the ghost
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            lights.SetActive(false);
        }
        else if (collision.CompareTag("Ghost"))
        {
            //Add the logic for changing the temperature and EMF based on the ghost
        }
    }
}