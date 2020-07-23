using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitV2 : MonoBehaviour
{

    public Transform target;
    Transform lastTarget;
    public float speed = 10;
    Vector3[] path;
    int targetIndex;
    public bool showPath = true;

    public void Start()
    {
        if (target != null && target != lastTarget)
        {
            StackRequestV2.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
            lastTarget = target;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0 && this != null)
        {
            targetIndex = 0;
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (showPath)
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(path[i], Vector3.one);
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
}
