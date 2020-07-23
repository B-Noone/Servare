using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class StackRequestV2 : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();
    static StackRequestV2 instance;
    AStarAlgoV2 pathfinding;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<AStarAlgoV2>();
    }

    void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.GetPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 tempStart, Vector3 tempEnd, Action<Vector3[], bool> tempCallBack)
    {
        pathStart = tempStart;
        pathEnd = tempEnd;
        callback = tempCallBack;
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}
