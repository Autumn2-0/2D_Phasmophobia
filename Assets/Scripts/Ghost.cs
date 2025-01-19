using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType
{
    
}

public enum Evidence
{

}

public class Ghost : MonoBehaviour
{
    public bool FreezingTemps;
    public bool EMF;
    public bool UV;
    public bool Dots;
    public bool SpiritBox;
    public bool GhostOrbs;
    public bool GhostWriting;
    public bool MS;
    public bool Scratching;
    public bool Hallucinations;

    public bool activeEMF;
    public bool activeUV;
    public bool activeDots;
    public bool activeMS;

    [Header("Area")]
    public Room ghostRoom;
    public Room currentRoom;
    public int temperatureModifier = -25;

    [Header("Movement")]
    public float roamingSpeed = 3.5f;
    public float huntingSpeed = 7f;
    public bool hunting;
    public float huntDuration;


    private void Update()
    {
        Vector2 dirToPlayer = (transform.position - GameManager.player.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer);

        if (hit.collider.CompareTag("Player"))
        {
            StartCoroutine(HuntSequence());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom = collision.GetComponent<Room>();
        }
    }

    public IEnumerator HuntSequence()
    {
        hunting = true;

        //Change to hunting behaviour

        yield return new WaitForSeconds(huntDuration);

        //Change to Roaming behaviour

        hunting = false;
        ghostRoom = currentRoom;
    }
}