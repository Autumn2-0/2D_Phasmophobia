using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMF : Item
{
    float dotProduct;
    float distance;
    public float minDot = 0.25f;
    public float maxDistance = 5;
    float percent;
    float displayEMF;
    public bool directional = true;
    protected override void UpdateItem()
    {
        dotProduct = Vector2.Dot(transform.up, (GameManager.ghost.transform.position - transform.position).normalized);
        distance = (GameManager.ghost.transform.position - transform.position).magnitude;

        if (!directional)
            dotProduct = 1;

        displayEMF = 1;
        if (dotProduct > minDot && distance < maxDistance)
        {
            percent = dotProduct * Mathf.Sqrt((maxDistance - distance) / maxDistance);
            float multiplier = 3.5f;
            if (GameManager.ghost.GetComponent<Ghost>().activeEMF)
                multiplier += 1.49f;
            if (GameManager.ghost.GetComponent<Ghost>().EMF)
                multiplier+=1.25f;

            displayEMF *= multiplier * percent;
            displayEMF = Mathf.FloorToInt(displayEMF);
            if (displayEMF <= 0) displayEMF = 1;
        }
        Debug.Log(displayEMF);
    }
}
