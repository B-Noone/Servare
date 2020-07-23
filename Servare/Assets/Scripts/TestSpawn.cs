using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour {
    public GameObject resource;
    public float spawnRangeNodes = 100.0f;
    public float spawnRangeResource = 20.0f;
    public int numberOfResource = 10;
    public int numberOfNodes = 5;
    public string resourceName;

    Vector3 position;
    float nodePosX = 0.0f;
    float nodePosZ = 0.0f;

	void Start () {
        resourceName = resource.name;
        Vector3 objectScale = resource.transform.lossyScale;
        float heightPos = (objectScale.y / 2);

        for (int j = 0; j < numberOfNodes; j++)
        {
            nodePosX = Random.Range(-spawnRangeNodes, spawnRangeNodes+1);
            nodePosZ = Random.Range(-spawnRangeNodes, spawnRangeNodes+1);
            for (int i = 0; i < numberOfResource; i++)
            {
                position = new Vector3(nodePosX + Random.Range(-spawnRangeResource, spawnRangeResource+1), heightPos, nodePosZ + Random.Range(-spawnRangeResource, spawnRangeResource+1));
                GameObject newResource = Instantiate(resource, position, Quaternion.identity);
                newResource.name = resourceName;
                newResource.transform.SetParent(gameObject.transform);
            }
        }
       // GameObject.Find("ObjectLists").GetComponent<ListOfObjects>().CheckResources();
	}
}
