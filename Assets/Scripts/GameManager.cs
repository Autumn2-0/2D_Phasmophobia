using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameObject player;
    public static GameObject ghost;
    public static GhostType type;
    public static Camera mainCamera;
    public static Vector3 mouseWorldPosition;

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
        Debug.Log(type.ToString());     //Debug
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mouseWorldPosition = GetMouseWorldPosition();
    }

    public enum GhostType
    {
        Spirit,
        Demon,
        Myling
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0; // Ensure z is set correctly for 2D
        return mouseWorldPosition;
    }
}
