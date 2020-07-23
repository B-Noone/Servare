using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarAlgoV2 : MonoBehaviour
{
    Grid grid;

    Vector3[] waypoints;
    bool pathSuccess = false;
    Nodes firstNode;
    Nodes targetNode;

    Heap<Nodes> openSet;
    HashSet<Nodes> closeSet;

    List<Nodes> path;

    Vector2 directionOld;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    public void GetPath(PathRequest request, Action<PathResult> callback)
    {
        waypoints = new Vector3[0];
        pathSuccess = false;
        firstNode = grid.NodeWorldPoint(request.pathStart);
        targetNode = grid.NodeWorldPoint(request.pathEnd);
        if (firstNode.walkable && targetNode.walkable)
        {
            //Heap<Nodes> openSet = new Heap<Nodes>(grid.MaxSize);
            //HashSet<Nodes> closeSet = new HashSet<Nodes>();
            openSet = new Heap<Nodes>(grid.MaxSize);
            closeSet = new HashSet<Nodes>();

            openSet.Add(firstNode);

            while (openSet.Count > 0)
            {
                Nodes currentNode = openSet.RemoveFirst();

                closeSet.Add(currentNode);
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Nodes adjacent in grid.GetAdjacent(currentNode))
                {
                    if (!adjacent.walkable || closeSet.Contains(adjacent))
                    {
                        continue;
                    }

                    int newMovementToAdjacent = currentNode.gCost + GetDistance(currentNode, adjacent);
                    if (newMovementToAdjacent < adjacent.gCost || !openSet.Contains(adjacent))
                    {
                        adjacent.gCost = newMovementToAdjacent;
                        adjacent.hCost = GetDistance(adjacent, targetNode);
                        adjacent.parent = currentNode;

                        if (!openSet.Contains(adjacent))
                        {
                            openSet.Add(adjacent);
                        }
                        else
                        {
                            openSet.UpdateItem(adjacent);
                        }
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = BackTrack(firstNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    Vector3[] BackTrack(Nodes firstNode, Nodes lastNode)
    {
        //List<Nodes> path = new List<Nodes>();
        path = new List<Nodes>();
        Nodes currentNode = lastNode;

        while (currentNode != firstNode)
        {
            if (currentNode == firstNode)
            {
                path.Add(currentNode);
            }
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Nodes> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        //Vector2 directionOld = Vector2.zero;
        directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridXPos - path[i].gridXPos, path[i - 1].gridYPos - path[i].gridYPos);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPos);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Nodes nodeA, Nodes nodeB)
    {
        int xDistance = Mathf.Abs(nodeA.gridXPos - nodeB.gridXPos);
        int yDistance = Mathf.Abs(nodeA.gridXPos - nodeB.gridYPos);

        if (xDistance > yDistance)
        {
            return 14 * yDistance + 10 * (xDistance - yDistance);
        }
        else
        {
            return 14 * xDistance + 10 * (yDistance - xDistance);
        }
    }
}
