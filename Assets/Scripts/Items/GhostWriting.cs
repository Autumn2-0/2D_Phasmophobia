using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWriting : Item
{
    public SpriteRenderer visuals;
    private static List<GhostWriting> books = new();

    //Art
    public Color closed;
    public Color open;
    public Color written;

    //Writing Frequency
    private static float avgTime;
    private static float timeVar;

    //Writing Stats
    public static float maxRange = 5f;
    public static float writingChance = 0.6f;

    //Bools
    private bool mainBook = false;
    private bool writing = false;

    //Timer
    public float nextUpdateTime = 0f;

    // Update is called once per frame
    protected override void StartItem()
    {
        books.Add(this);
        mainBook = books.IndexOf(this) == 0;
        avgTime = GameManager.ghost.stats.averageWritingTime;
        timeVar = GameManager.ghost.stats.variationWritingTime;
    }

    protected override void UpdateItem()
    {
        if (Time.time < nextUpdateTime || !mainBook)
        {
            return;
        }

        // Wait for a random interval
        float interval = Random.Range(avgTime - timeVar, avgTime + timeVar);
        nextUpdateTime += interval;

        //Check for Writing
        GhostWriting potentialBook = null;
        float bestDistance = maxRange;
        foreach (GhostWriting book in books)
        {
            float distance = Vector2.Distance(book.transform.position, GameManager.ghost.transform.position);
            if (book.placed && distance < maxRange)
            {
                if (potentialBook == null)
                {
                    potentialBook = book;
                    bestDistance = distance;
                }
                if (distance <= bestDistance)
                {
                    potentialBook = book;
                    bestDistance = distance;
                }
            }
        }

        if (potentialBook != null)
            Debug.Log("Potential Book Found");
        else
            Debug.Log("Potential Book Not Found");

        if (potentialBook != null && Random.Range(0f, 1f) < writingChance)
        {
            if (GameManager.ghost.GhostWriting)
            {
                potentialBook.writing = true;
                potentialBook.Interaction();
            }
            else
            {
                potentialBook.rb.velocity = potentialBook.throwForce * new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                potentialBook.placed = false;
                potentialBook.Interaction();
            }
        }
    }

    protected override void Interaction()
    {
        if (!placed)
        {
            visuals.color = closed;
        }
        else if (writing)
        {
            visuals.color = written;
        }
        else
        {
            visuals.color = open;
        }
    }
}
