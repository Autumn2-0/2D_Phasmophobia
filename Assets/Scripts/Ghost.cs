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
    public GhostType type;

    [Header("Area")]
    public Room ghostRoom;
    public Room currentRoom;
    public int temperatureModifier = -25;

    [Header("Roaming Movement")]
    public float roamingSpeed = 3.5f;
    [Header("Hunting Movement")]
    public float stopDistance = 1f;
    public float huntingSpeed = 7f;
    public bool hunting = false;
    public float huntDuration = 60;
    public float huntCooldown = 300;
    public float huntMaxCooldown = 300;

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
        System.Array values = System.Enum.GetValues(typeof(GhostType));
        type = (GhostType)values.GetValue(Random.Range(0, values.Length));
        Debug.Log(type.ToString());
        huntCooldown = huntMaxCooldown;
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
        if (Vector2.Distance(transform.position, GameManager.player.transform.position) >= stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.player.transform.position, huntingSpeed * Time.deltaTime);
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

        yield return new WaitForSeconds(huntDuration);

        //Change to Roaming behaviour

        hunting = false;
        ghostRoom = currentRoom;
        huntCooldown = huntMaxCooldown;
        Debug.Log("Finished Hunt");
    }
}