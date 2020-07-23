using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceControl : MonoBehaviour {
    public int minResource = 1;
    public int maxResource = 3;
    public int numberOfResource;

    void Start () {
        numberOfResource = Random.Range(minResource, maxResource+1);
	}
	
	void Update () {
		if(numberOfResource <= 0)
        {
            Destroy(gameObject);
        }
	}

    //void OnMouseDown()
    //{
    //    Destroy(gameObject);
    //}
}
