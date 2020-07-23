using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StackRequest : MonoBehaviour {

    Queue<PathRequest> pathRequestionStack = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static StackRequest instance;
    AStarAlgo pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<AStarAlgo>();
    }

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestionStack.Enqueue(newRequest);
        instance.NextProcess();
    }

    void NextProcess()
    {
        if(!isProcessingPath && pathRequestionStack.Count > 0)
        {
            currentPathRequest = pathRequestionStack.Dequeue();
            isProcessingPath = true;
            pathfinding.StartGetPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        NextProcess();
    }

    struct PathRequest
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
}
