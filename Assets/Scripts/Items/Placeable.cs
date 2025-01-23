using System.Collections;
using UnityEngine;

public class Placeable : MonoBehaviour
{

    private PickUp pickUp;
    public bool placed = false;

    private void Start()
    {
        pickUp = GetComponent<PickUp>();
    }

    public bool Place()
    {
        bool canReach = Interact.instance.CanReach(GameManager.player.transform.position, GameManager.mouseWorldPosition, GameManager.player.playerReach);
        if (canReach)
        {
            pickUp.UnequipItem();
            transform.position = new Vector3(GameManager.mouseWorldPosition.x, GameManager.mouseWorldPosition.y, transform.position.z);
            StartCoroutine(Placing());
            placed = true;
            return true;
        }
        return false;
    }
    private IEnumerator Placing()
    {
        while (true)
        {
            // Rotate to face the mouse
            transform.up = GameManager.mouseWorldPosition - (Vector2)transform.position;

            if (Vector2.Distance(GameManager.player.transform.position, transform.position) > GameManager.player.playerReach || Vector2.Distance(GameManager.mouseWorldPosition, transform.position) > 1f)
            {
                yield break;
            }
            yield return null;
        }
    }
}
