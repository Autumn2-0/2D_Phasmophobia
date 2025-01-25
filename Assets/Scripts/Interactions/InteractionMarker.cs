using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMarking : MonoBehaviour
{
    public int EMF;
    public static List<InteractionMarking> Interactions = new List<InteractionMarking> ();
    public static float defaultDuration = 5f;

    public static void Instantiate(GameObject interaction, int EMF, float duration = 0f)
    {
        InteractionMarking i = interaction.AddComponent<InteractionMarking>();
        i.Initiate(EMF, duration);
    }

    private void Initiate(int value, float duration = 0f)
    {
        Interactions.Add(this);
        if (duration == 0)
        {
            duration = defaultDuration;
        }
        StartCoroutine(Expire(duration));

        EMF = value;

        if (GameManager.ghost.stats.EMF)
        {
            if (Random.value < GameManager.ghost.stats.chanceOfEMF + 0.05f * (value - 3))
            {
                EMF = 5;
            }
        }

        int i = 0;
        foreach (InteractionMarking interaction in Interactions)
        {
            if (interaction.EMF > EMF)
                i++;
            else
                break;
        }
        Interactions.Insert(i, this);
    }

    // Coroutine that waits for the expiration time before deactivating or destroying the object
    private IEnumerator Expire(float duration)
    {
        yield return new WaitForSeconds (duration);
        Interactions.Remove(this);
        Destroy(this);
    }
}
