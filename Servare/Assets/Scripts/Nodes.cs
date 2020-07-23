using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : IHeapItem<Nodes>{

    public bool walkable;
    public Vector3 worldPos;
    public int gridXPos;
    public int gridYPos;

    public int gCost;
    public int hCost;
    public Nodes parent;
    int heapIndex;

    public Nodes(bool tempWalkable, Vector3 tempWorldPos, int tempGridXPos, int tempGridYPos)
    {
        walkable = tempWalkable;
        worldPos = tempWorldPos;
        gridXPos = tempGridXPos;
        gridYPos = tempGridYPos;
    }

    public int getfCost()
    {
        return gCost + hCost;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Nodes nodeToCompare)
    {
        int compare = getfCost().CompareTo(nodeToCompare.getfCost());
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
