using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 20;
    Vector2[] path;
    int targetIndex;
    private Rigidbody2D rb;
    

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        rb = GetComponent<Rigidbody2D>();
    }

    public int GetMovementPenalty()
    {
        return Grid.instance.NodeFromWorldPoint(transform.position).movementPenalty;
    }

    public bool ReachedDestination(float stoppingDistance)
    {
        return Vector2.Distance(transform.position, target.transform.position) < stoppingDistance;
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && newPath != null)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            GameManager.ghost.noPath = true;
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

        while (true)
        {
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            //transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            rb.velocity = (currentWaypoint - (Vector2)transform.position).normalized * speed;
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector2.one * 0.1f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}