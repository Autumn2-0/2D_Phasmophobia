using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public GameObject ghost;
    public GhostType type; 
    
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
        ghost = GameObject.FindGameObjectWithTag("Ghost");
        System.Array values = System.Enum.GetValues(typeof(GhostType));
        type = (GhostType)values.GetValue(Random.Range(0, values.Length));
        Debug.Log(type.ToString());
    }

    public enum GhostType
    {
        Spirit,
        Demon,
        Myling
    }
}
