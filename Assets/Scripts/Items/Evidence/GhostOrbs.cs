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
        return RoomManager.Instance.ghostRoom.GetRandomPointInRoom();
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
