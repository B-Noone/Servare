using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfObjects : MonoBehaviour {

    public GameObject[] resourceList;
    public GameObject[] rockList;
    public GameObject[] treeList;
    public GameObject[] bushList;
    public GameObject[] jimList;
    public GameObject[] buildingList;
    public GameObject[] constructionList;
    public int ArraySize = 1000;

	// Use this for initialization
	void Start () {
        rockList = new GameObject[ArraySize];
        treeList = new GameObject[ArraySize];
        bushList = new GameObject[ArraySize];
        //buildingList = new GameObject[ArraySize];
        //constructionList = new GameObject[ArraySize];
        CheckResources();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        RefreshList();
    }

    void RefreshList()
    {
        CheckResources();
        FindObjectName("Rock", rockList);
        FindObjectName("Tree", treeList);
        FindObjectName("Bush", bushList);
    }

    void FindObjectName(string type, GameObject[] objList)
    {
        int listNum = 0; //Position in array
        for (int i = 0; i < resourceList.Length; i++)
        {
            if (resourceList[i] != null && resourceList[i].name == type)
            {
                objList[listNum] = resourceList[i];
                listNum += 1;
            }
        }
        for(int j = listNum; j < objList.Length; j++) //Clear any objects that don't exist
        {
            objList[j] = null;
        }
    }

    public void CheckResources() //Finds all resources in the world
    {
        resourceList = GameObject.FindGameObjectsWithTag("Resource");
        jimList = GameObject.FindGameObjectsWithTag("AI");
        buildingList = GameObject.FindGameObjectsWithTag("Structure");
        constructionList = GameObject.FindGameObjectsWithTag("Construction");
    }

    //public void AddConstruction(GameObject cObj)
    //{
    //    for(int i = 0; i < constructionList.Length; i++)
    //    {
    //        if(constructionList[i] == null)
    //        {
    //            constructionList[i] = cObj;
    //            break;
    //        }
    //    }
    //}

    //public void AddBuilding(GameObject bObj)
    //{
    //    for (int i = 0; i < buildingList.Length; i++)
    //    {
    //        if (buildingList[i] == null)
    //        {
    //            buildingList[i] = bObj;
    //            break;
    //        }
    //    }
    //}
}