using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterScript : MonoBehaviour {

    List<GameObject> planters = new List<GameObject>();
    public GameObject respawnObject;
    private bool runningTimer = false;
    public float respawnTime = 10.0f;
    StoredObject script;

    void Start () {
        foreach(Transform child in transform)
        {
            planters.Add(child.gameObject);
        }
	}

    void Update()
    {
        if (runningTimer == false)
        {
            if (CheckSpawners())
            {
                StartCoroutine(timer(respawnTime, RespawnObject));
            }
        }
    }

    bool CheckSpawners()
    {
        for(int i = 0; i < planters.Count; i++)
        {
            script = planters[i].GetComponent<StoredObject>();
            if (!script.empty)
            {
                return false;
            }
        }
        return true;
    }

    void RespawnObject()
    {
        for(int i = 0; i < planters.Count; i++)
        {
            script = planters[i].GetComponent<StoredObject>();
            Vector3 tempPos = planters[i].transform.position;
            tempPos.y = respawnObject.transform.lossyScale.y / 2;
            GameObject newObj = Instantiate(respawnObject, tempPos, Quaternion.identity);
            newObj.transform.SetParent(GameObject.Find(respawnObject.name + "s").transform);
            newObj.name = respawnObject.name;
            script.setPossesion(newObj);
        }
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time);
        funcToRun();
        runningTimer = false;
    }
}
