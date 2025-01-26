using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostOrbs : Item
{
    public static bool inUse = false;
    private static Coroutine spawnOrbs;
    public static GameObject ghostOrbs;
    public GameObject ghostOrbsPrefab;
    protected override void StartItem()
    {
        if (spawnOrbs == null)
        {
            // Start the coroutine only if none is running
            spawnOrbs = StartCoroutine(SpawnOrbs());
            ghostOrbs = ghostOrbsPrefab;
        }
    }

    protected override void UpdateItem()
    {

    }

    public static IEnumerator SpawnOrbs()
    {
        if (GameManager.ghost.stats.GhostOrbs)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(GameManager.ghost.stats.orbsSpawnMin, GameManager.ghost.stats.orbsSpawnMax));
                Instantiate(ghostOrbs, GetRandomPointInGhostRoom(), new Quaternion());
                
            }
        }
        yield return null;
    }

    public static Vector2 GetRandomPointInGhostRoom()
    {
        PolygonCollider2D polygonCollider = RoomManager.Instance.ghostRoom.GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D reference is missing!");
            return Vector2.zero;
        }

        // Get the bounds of the collider
        Bounds bounds = polygonCollider.bounds;

        // Try to find a random point within the polygon
        for (int i = 0; i < 100; i++) // Limit attempts to prevent infinite loops
        {
            // Generate a random point within the bounds
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 randomPoint = new Vector2(randomX, randomY);

            // Check if the point is inside the polygon
            if (polygonCollider.OverlapPoint(randomPoint))
            {
                return randomPoint; // Return the valid random point
            }
        }

        return Vector2.zero; // Return a default value if no point was found
    }

    public override int GhostInteraction(bool itemSpecific)
    {
        return base.GhostInteraction();
    }

    protected override void Interaction()
    {
        inUse = active && inHand;
        NightVisionPostProcessing.Instance.SetVolume(inUse);
    }
}
