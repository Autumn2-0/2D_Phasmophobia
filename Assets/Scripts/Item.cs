using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected int uses = -1;
    protected bool active = false;
    protected float placeDistance = 5;
    protected float throwForce = 2f;
    protected bool canPlace = true;
    protected Rigidbody2D rb;
    private Coroutine placingCoroutine;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Place()
    {
        if (canPlace && Vector2.Distance(GameManager.player.transform.position, GameManager.mouseWorldPosition) > placeDistance)
            return;

        transform.SetParent(null);
        transform.position = new Vector3(GameManager.mouseWorldPosition.x, GameManager.mouseWorldPosition.y, transform.position.z);
        placingCoroutine = StartCoroutine(placing());
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
