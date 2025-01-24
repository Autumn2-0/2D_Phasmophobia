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
        StartCoroutine(ExpireAfterTime());
    }

    public void Initiate(int value)
    {
        EMF = value;
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
    private IEnumerator ExpireAfterTime()
    {
        yield return new WaitForSeconds(expirationTime);
        Interactions.Remove(this);
        Destroy(this);
    }
}
