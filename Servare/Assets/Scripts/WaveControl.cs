using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveControl : MonoBehaviour {

    CoreHealth coreObjHealth;
    private bool runningTimer = false;
    public float freeTime = 60.0f;
    public float timeUntilWave = 0;
    public int waveNum = 0;
    public GameObject EvilJim;
    public List<GameObject> listOfEJ;

    void Start()
    {
        coreObjHealth = gameObject.GetComponent<CoreHealth>();
        StartCoroutine(timer(freeTime, StartWaveTimer));
        listOfEJ = new List<GameObject>();
    }

    void Update()
    {
        if(timeUntilWave != 0)
        {
            if (runningTimer == false)
            {
                StartCoroutine(timer(1, CountDown));
            }
        }
        else if(coreObjHealth.coreHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
        else if(listOfEJ.Count == 0)
        {
            timeUntilWave = freeTime;
        }
        else if(timeUntilWave == 0)
        {
            CheckEJs();
        }
    }

    void CheckEJs()
    {
        for (int i = 0; i < listOfEJ.Count; i++) {
            if (listOfEJ[i] == null)
            {
                listOfEJ.Remove(listOfEJ[i]);
            }
        }
    }

    void StartWaveTimer()
    {
        timeUntilWave = freeTime;
    }

    void CountDown()
    {
        timeUntilWave--;
        if (timeUntilWave <= 0)
        {
            Wave();
        }
    }

    void Wave()
    {
        Debug.Log("WAVE ACTIVE");
        int numberOfEnemies = Mathf.Abs((waveNum * 3) + 3);
        for(int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPos = new Vector3(transform.position.x + Random.Range(290, 300), transform.position.y, transform.position.z + Random.Range(-200, 200));
            GameObject newEJ = Instantiate(EvilJim, randomPos, Quaternion.identity);
            newEJ.name = EvilJim.name;
            listOfEJ.Add(newEJ);
        }
        waveNum++;
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time);
        funcToRun();
        runningTimer = false;
    }
}
