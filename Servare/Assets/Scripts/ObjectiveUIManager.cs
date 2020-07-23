using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUIManager : MonoBehaviour {
    GameObject coreObj;
    CoreHealth coreObjHealth;
    WaveControl waveScript;
    Text coreHealthTxt;
    Text nextWaveTxt;

    public float countDown;

    // Use this for initialization
    void Start () {
        coreObj = GameObject.Find("Core");
        coreObjHealth = coreObj.GetComponent<CoreHealth>();
        waveScript = coreObj.GetComponent<WaveControl>();
        coreHealthTxt = GameObject.Find("/CanvasObjectives/CoreHealthTxt").GetComponent<Text>();
        nextWaveTxt = GameObject.Find("/CanvasObjectives/NextWaveTxt").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        UpdateHealth();
        UpdateWave();
    }

    void UpdateHealth()
    {
        coreHealthTxt.text = "Core Health: " + coreObjHealth.coreHealth;
    }

    void UpdateWave()
    {
        nextWaveTxt.text = "Next Wave: " + waveScript.timeUntilWave;
    }

}
