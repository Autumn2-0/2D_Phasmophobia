using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private bool pickUpAllowed = false;
    private GameObject player;
    private bool sightLine = false; //checks if the player is close enough

    private void Start()
    {
        player = GameManager.player.gameObject;
    }

    void Update()
    {
        if (sightLine && Input.GetKeyDown(KeyCode.R))
        {
            PickUp(this.GameObject());
        }
    }

    //Checks the distance between the item and the player
    private void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if(ray.collider != null)
        {
            float rayDistance = ray.distance;
            if (rayDistance < 0.6f)
            {
                sightLine = true;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                sightLine = false;
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);

            }
/*
            sightLine = ray.collider.CompareTag("Player");
            Debug.Log(ray.collider.tag);
            if (sightLine)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }*/
        }
    }

    //checks if player has collided with item
  /*  private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            pickUpAllowed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        pickUpAllowed = false;
    }
  */

    //Pickups item and adds to inventory
    private void PickUp(GameObject item)
    {
        Debug.Log(item);
        //goes to inventory
        Destroy(gameObject);//temp
    }
}