using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagement : MonoBehaviour {
    Text scoreTxt;
    private bool runningTimer = false;
    static public int score = 0;

    // Use this for initialization
    void Start () {
        scoreTxt = GameObject.Find("/CanvasObjectives/ScoreTxt").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreTxt.text = "Score: " + score;
        if (runningTimer == false)
        {
            StartCoroutine(timer(10, IncrementScore));
        }
    }

    void IncrementScore()
    {
        score += 20;
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time);
        funcToRun();
        runningTimer = false;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
