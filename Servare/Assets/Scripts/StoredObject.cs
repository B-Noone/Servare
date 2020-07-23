using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredObject : MonoBehaviour {
    public GameObject possesion;
    public bool empty = true;

    void Update()
    {
        if(empty == false)
        {
            if(possesion == null)
            {
                empty = true;
            }
        }
    }

    public void setPossesion(GameObject tempObj)
    {
        possesion = tempObj;
        empty = false;
    }
}
