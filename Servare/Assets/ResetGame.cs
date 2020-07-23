using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour {

    ScoreManagement sM;
	// Use this for initialization
	void Start () {
        sM = GameObject.Find("CanvasObjectives").GetComponent<ScoreManagement>();
        sM.ResetScore();
	}
}
