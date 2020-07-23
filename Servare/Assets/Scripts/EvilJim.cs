using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilJim : MonoBehaviour {
    public float health = 100;
    public float moveSpeed = 15.0f;
    public float interactionRange = 10.0f;
    GameObject coreObj;
    CoreHealth coreObjHealth;
    private bool runningTimer = false;

    void Start()
    {
        coreObj = GameObject.Find("Core");
        coreObjHealth = coreObj.GetComponent<CoreHealth>();
    }

    void Update()
    {
        EJCore();
    }

    void EJCore()
    {
        if (!Destination())
        {
            transform.position = Vector3.MoveTowards(transform.position, coreObj.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (runningTimer == false)
            {
                StartCoroutine(timer(1, DamageCore));
            }
        }
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void DamageCore()
    {
        if(gameObject != null)
        {
            coreObjHealth.coreHealth--;
        }
    }

    bool Destination() //Check if AI is at Objective
    {
        if (((transform.position.x <= coreObj.transform.position.x + interactionRange) && (transform.position.x >= coreObj.transform.position.x - interactionRange)) && ((transform.position.z <= coreObj.transform.position.z + interactionRange) && (transform.position.z >= coreObj.transform.position.z - interactionRange)))
        {
            return true;
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
}
