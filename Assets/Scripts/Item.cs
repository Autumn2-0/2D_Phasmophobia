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
    protected float placeDistance = 5;
    protected float throwForce = 2f;
    protected bool canPlace = true;
    protected bool held = false;
    protected bool pocketable = false;
    protected bool equipped = false;
    protected Rigidbody2D rb;
    private Coroutine placingCoroutine;
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
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ItemStart();
    }
    protected abstract void ItemStart();
    public void Update()
    {
        if (equipped || !held || pocketable)
        {
            if (active)
                UpdateItem();
        }    
        else
            gameObject.SetActive(false);
    }

    public void EquipItem()
    {
        gameObject.SetActive(true);
        Show(transform);
    }
    public void PocketItem()
    {
        if (!pocketable)
            gameObject.SetActive(true);
        Hide(transform);
    }
    protected abstract void UpdateItem();
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
        rb.AddForce(transform.up * throwForce);
        held = false;
        equipped = false;
    }
    public virtual void Use()
    {
        if (!active)
            if (uses != 0)
            {
                active = true;
                uses--;
            }
            else active = false;
    }
}
