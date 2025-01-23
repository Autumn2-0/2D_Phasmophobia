using UnityEngine;
using UnityEngine.Rendering;

public class PickUp : MonoBehaviour
{
    protected bool inHand = false;
    protected bool equipped = false;

    protected Rigidbody2D rb;
    protected Placeable placeable;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        placeable = GetComponent<Placeable>();
    }

    public void Update()
    {
        if (equipped)
        {
            transform.localPosition = Vector2.zero;
            transform.up = transform.parent.up;
        }
    }

    protected virtual void Interaction()
    {
        return;
    }

    public virtual int GhostInteraction(bool itemSpecific = false)
    {
        Throw(Random.Range(GameManager.ghost.stats.throwForceMin, GameManager.ghost.stats.throwForceMax), false);
        Interaction();
        return 2;
    }

    public void Throw(float force, bool forward)
    {
        Vector2 direction = transform.up;
        if (!forward)
        {
            direction = Random.insideUnitCircle.normalized;
        }
        rb.velocity = transform.up * force;
        if (placeable != null)
        {
            placeable.placed = false;
        }
        Interaction();
    }

    public void EquipItem(bool mainHand)
    {
        transform.SetParent(GameManager.ActiveItemSlot);
        inHand = mainHand;
        equipped = true;
        Visible(transform, inHand);
        Interaction();
    }

    public void UnequipItem()
    {
        transform.SetParent(null);
        Visible(transform, true);
        equipped = false;
        inHand = false;
        Interaction();
    }

    private void Visible(Transform objTransform, bool isVisible)
    {
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = isVisible;  // Makes the GameObject invisible
        }

        // Recursively do the same for all children
        foreach (Transform child in objTransform)
        {
            Visible(child, isVisible);  // Recursive call to handle all children
        }
    }
}
