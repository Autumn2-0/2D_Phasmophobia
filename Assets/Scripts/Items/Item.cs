using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected int uses = -1;
    [SerializeField]
    protected bool active = false;
    protected float placeDistance = 2;
    [SerializeField]
    protected float throwForce = 3f;
    protected bool canPlace = true;
    protected bool held = false;
    [SerializeField]
    protected bool pocketable = false;
    [SerializeField]
    protected bool alwaysUpdate = false;
    protected bool equipped = false;
    protected Rigidbody2D rb;
    private Coroutine placingCoroutine;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartItem();
    }

    protected virtual void StartItem() { return; }

    public void Update()
    {
        if (equipped || !held || pocketable)
        {
            if (active || alwaysUpdate)
                UpdateItem();
        }    
        else
            gameObject.SetActive(false);
        if (held)
        {
            transform.localPosition = Vector3.zero;
            transform.up = transform.parent.up;
        } 
    }

    public void EquipItem()
    {
        gameObject.SetActive(true);
        equipped = true;
        held = true;
        Show(transform);
        NewParent();
    }
    public void PocketItem()
    {
        if (!pocketable)
            gameObject.SetActive(true);
        equipped = false;
        held = true;
        Hide(transform);
        NewParent();
    }
    private void NewParent()
    {
        transform.SetParent(GameManager.ActiveItemSlot);
    }
    protected virtual void UpdateItem() { return; }
    public void Place()
    {
        if (canPlace && Vector2.Distance(GameManager.player.transform.position, GameManager.mouseWorldPosition) > placeDistance)
            return;

        transform.SetParent(null);
        transform.position = new Vector3(GameManager.mouseWorldPosition.x, GameManager.mouseWorldPosition.y, transform.position.z);
        placingCoroutine = StartCoroutine(placing());
        held = false;
        equipped = false;
    }
    private IEnumerator placing()
    {
        while (true)
        {
            // Rotate to face the mouse
            transform.up = GameManager.mouseWorldPosition - transform.position;

            if (Input.GetMouseButtonDown(0) || Vector2.Distance(GameManager.player.transform.position, transform.position) > placeDistance)
            {
                StopCoroutine(placingCoroutine);
                placingCoroutine = null;
            }
            yield return null;
        }
    }
    public void Drop()
    {
        transform.SetParent(null);
        transform.up = GameManager.mouseWorldPosition - transform.position;
        rb.velocity = transform.up * throwForce;
        held = false;
        equipped = false;
    }
    public virtual void Use()
    {
        if (!active)
            if (uses != 0)
            {
                active = true;
                Toggled(true);
                uses--;
            }
            else
            {
                active = false;
            }
        else
        {
            active = false;
            Toggled(false);
        }
    }

    protected virtual void Toggled(bool on) { return; }

    private void Show(Transform objTransform)
    {
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;  // Makes the GameObject invisible
        }

        // Recursively do the same for all children
        foreach (Transform child in objTransform)
        {
            Show(child);  // Recursive call to handle all children
        }
    }
    private void Hide(Transform objTransform)
    {
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;  // Makes the GameObject invisible
        }

        // Recursively do the same for all children
        foreach (Transform child in objTransform)
        {
            Hide(child);  // Recursive call to handle all children
        }
    }
}
