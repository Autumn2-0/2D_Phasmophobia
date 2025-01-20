using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private bool pickUpAllowed = false;

    void Update()
    {
        Debug.Log(pickUpAllowed);
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.R))
        {
            PickUp();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            pickUpAllowed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        pickUpAllowed = false;
    }

    private void PickUp()
    {
        //goes to inventory
        Destroy(gameObject);
    }
}