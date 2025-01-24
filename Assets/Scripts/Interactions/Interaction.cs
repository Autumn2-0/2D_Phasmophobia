using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public static float expirationTime = 5f; // Time before the object expires (default is 5 seconds)
    public int EMF;
    public static List<Interaction> Interactions = new List<Interaction> ();

    void Start()
    {
        // Start the coroutine to handle expiration
        StartCoroutine(Expire());
    }

    public void Initiate(int value)
    {
        EMF = value;

        if (GameManager.ghost.stats.EMF)
        {
            if (Random.value < GameManager.ghost.stats.chanceOfEMF + 0.05f * (value - 3))
            {
                EMF = 5;
            }
        }

        int i = 0;
        foreach (Interaction interaction in Interactions)
        {
            if (interaction.EMF > EMF)
                i++;
            else
                break;
        }
        Interactions.Insert(i, this);
    }

    // Coroutine that waits for the expiration time before deactivating or destroying the object
    private IEnumerator Expire()
    {
        yield return new WaitForSeconds (expirationTime);
        Interactions.Remove(this);
        Destroy(this);
    }
}
