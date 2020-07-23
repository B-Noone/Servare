using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile : MonoBehaviour {
    public int wood;
    public int stone;
    public int food;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    int GetWood()
    {
        return wood;
    }

    int GetStone()
    {
        return stone;
    }

    int GetFood()
    {
        return food;
    }

    void SetWood(int woodNum)
    {
        wood = woodNum;
    }

    void SetStone(int stoneNum)
    {
        stone = stoneNum;
    }

    void SetFood(int foodNum)
    {
        food = foodNum;
    }
}
