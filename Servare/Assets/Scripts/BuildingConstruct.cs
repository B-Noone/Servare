using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruct : MonoBehaviour {
    public int wood;
    public int stone;
    public GameObject Building;

    void Update()
    {
        Completed();
    }

    public void Completed()
    {
        if (wood == 0 && stone == 0)
        {
            GameObject newBuilding = Instantiate(Building);
            GameObject BuildingList = GameObject.Find("ObjectLists");
            newBuilding.name = Building.name;
            newBuilding.transform.position = new Vector3(gameObject.transform.position.x, Building.transform.lossyScale.y/2, gameObject.transform.position.z);
            Destroy(gameObject);
        }
    }
}
