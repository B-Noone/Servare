using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

    public float interactionRange = 50.0f;
    public GameObject target;
    List<GameObject> potentialTargets;
    GameObject[] tempObjArray;
    WaveControl currentWave;
    public float damage = 25;
    public float killDelay = 2.0f;

    private bool runningTimer = false;

    void Start()
    {
        potentialTargets = new List<GameObject>();
        currentWave = GameObject.Find("Core").GetComponent<WaveControl>();
    }

	// Update is called once per frame
	void Update () {
        if (currentWave.timeUntilWave == 0)
        {
            SearchForTargets();
            if (runningTimer == false)
            {
                StartCoroutine(timer(killDelay, KillTarget));
            }
        }
	}

    void SearchForTargets()
    {
        //Debug.Log("Searching targets");
        tempObjArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            potentialTargets.Add(tempObjArray[i]);
        }
    }

    void KillTarget()
    {
        Debug.Log("Attacking targets");
        for (int i = 0; i < potentialTargets.Count; i++)
        {
            if (Destination(potentialTargets[i]))
            {
                potentialTargets[i].GetComponent<EvilJim>().health -= damage;
                break;
            }
        }
    }

    bool Destination(GameObject temp) //Check if AI is at Objective
    {
        if (temp != null)
        {
            if (((transform.position.x <= temp.transform.position.x + interactionRange) && (transform.position.x >= temp.transform.position.x - interactionRange)) && ((transform.position.z <= temp.transform.position.z + interactionRange) && (transform.position.z >= temp.transform.position.z - interactionRange)))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time);
        funcToRun();
        runningTimer = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
