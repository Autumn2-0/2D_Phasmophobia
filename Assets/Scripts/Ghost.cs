using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType
{
    Spirit,
    Demon,
    Myling
}

public class Ghost : MonoBehaviour
{
    [Header("Basic Info")]
    public GhostType type;
    public GhostStats stats;

    [Header("Area")]
    public Room ghostRoom;
    public Room currentRoom;

    [Header("Roaming Movement")]

    [Header("Hunting Movement")]
    public bool hunting = false;
    public float huntCooldown = 300;

    public GhostStats[] possibleStats;

    [Header("Booleans")]
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

    private void Awake()
    {
        stats = possibleStats[Random.Range(0, possibleStats.Length)];
        type = stats.type;
        Debug.Log(type.ToString());

        huntCooldown = stats.huntMaxCooldown;

        GameManager.ghost = this;
    }

    private void Update()
    {
        if (huntCooldown > 0)
        {
            huntCooldown -= Time.deltaTime;
        }
        if (!hunting && huntCooldown <= 0)
        {
            StartCoroutine(HuntSequence());
        }

        switch (hunting)
        {
            case false:
                RoamingMovement();
                break;
            case true:
                HuntingMovement();
                break;
        }
    }

    public void RoamingMovement()
    {

    }

    public void HuntingMovement()
    {
        if (Vector2.Distance(transform.position, GameManager.player.transform.position) >= stats.stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.player.transform.position, stats.huntingSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom = collision.GetComponent<Room>();
            if (ghostRoom == null) ghostRoom = currentRoom;
        }
    }

    public IEnumerator HuntSequence()
    {
        Debug.Log("Started Hunt");
        hunting = true;

        //Change to hunting behaviour

        yield return new WaitForSeconds(stats.huntDuration);

        //Change to Roaming behaviour

        hunting = false;
        ghostRoom = currentRoom;
        huntCooldown = stats.huntMaxCooldown;
        Debug.Log("Finished Hunt");
    }
}