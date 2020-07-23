using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public GameObject objToSpawn;
    public GameObject bushObj;
    public GameObject rockObj;
    public GameObject treeObj;
    public GameObject storageObj;
    public GameObject foresterObj;
    public GameObject quarryObj;
    public GameObject farmObj;
    public GameObject towerObj;
    private Vector3 mousePos;
    private Vector3 objPos;
    private GameObject manaText;
    public GameObject selectPowerImg;
    private GameObject bushImg;
    private GameObject rockImg;
    private GameObject treeImg;
    public GameObject highlight;
    public float mana = 100;
    private bool toolBar = true;
    private float toolBarDrop = 150;
    //GameObject canvasTBAbilities;
    //GameObject canvasTBBuild;
    private bool runningTimer = false;

    public GameObject[] tabs;
    public int numOfTabs = 2;
    public int currentTab = 0;

    public GameObject[] toolSelect;
    public int numOfTools = 3;
    public int currentTool = 0;

    public GameObject[] buildSelect;
    public int numOfBuild = 4;
    public int currentBuild = 0;


    void Start()
    {
        Setup();
    }

    void Update()
    {
        UpdateUI();
        KeyControl();
        UpdateMana();
    }

    void Setup()
    {
        //tabs = new GameObject[numOfTabs];
        //bushImg = GameObject.Find("BushImg");
        //rockImg = GameObject.Find("RockImg");
        //treeImg = GameObject.Find("TreeImg");


        selectPowerImg = toolSelect[0];
        highlight = FindHighlight(0);
        //for(int i = 0; i < numOfTabs; i++)
        //{
        //    tabs[i] = GameObject.Find("CanvasToolbarAbilities"); ;
        //    tabs[i] = GameObject.Find("CanvasToolbarBuild");
        //}
    }

    void KeyControl()
    {
        if (toolBar == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (currentTab == 0)
                {
                    objToSpawn = bushObj;
                    selectPowerImg = toolSelect[0];
                }
                else
                {
                    objToSpawn = storageObj;
                    selectPowerImg = buildSelect[0];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (currentTab == 0)
                {
                    objToSpawn = rockObj;
                    selectPowerImg = toolSelect[1];
                }
                else
                {
                    objToSpawn = foresterObj;
                    selectPowerImg = buildSelect[1];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (currentTab == 0)
                {
                    objToSpawn = treeObj;
                    selectPowerImg = toolSelect[2];
                }
                else
                {
                    objToSpawn = quarryObj;
                    selectPowerImg = buildSelect[2];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (currentTab == 0)
                {
                    Debug.Log("No ability");
                }
                else
                {
                    objToSpawn = farmObj;
                    selectPowerImg = buildSelect[3];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (currentTab == 0)
                {
                    Debug.Log("No ability");
                }
                else
                {
                    objToSpawn = towerObj;
                    selectPowerImg = buildSelect[4];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                MoveBar(-toolBarDrop);
            }
        }
        else if(toolBar == false)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                MoveBar(toolBarDrop);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            SpawnGroupOfObj();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ChangeTab(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeTab(true);
        }
    }

    void ChangeTab(bool right)
    {
        if(right && currentTab == tabs.Length-1)
        {
            currentTab = 0;
            //highlight = GameObject.Find("/Highlight");
            highlight = FindHighlight(currentTab);
        }
        else if(!right && currentTab == 0){
            currentTab = tabs.Length-1;
            highlight = FindHighlight(currentTab);
        }
        else if(right)
        {
            currentTab += 1;
            highlight = FindHighlight(currentTab);
        }
        else
        {
            currentTab -= 1;
            highlight = FindHighlight(currentTab);
        }
        if(currentTab == 0)
        {
            objToSpawn = bushObj;
            selectPowerImg = toolSelect[0];
        }
        else
        {
            objToSpawn = storageObj;
            selectPowerImg = buildSelect[0];
        }
    }

    GameObject FindHighlight(int num)
    {
        return GameObject.Find(tabs[num].name + "/Highlight");
    }

    void MoveBar(float posY)
    {
        for (int i = 0; i < numOfTabs; i++)
        {
            Transform[] children = tabs[i].GetComponentsInChildren<Transform>();
            foreach (Transform tempChild in children)
            {
                tempChild.position = new Vector3(tempChild.position.x, tempChild.position.y + posY, tempChild.position.z);
            }
        }
        if (toolBar == true)
        {
            toolBar = false;
        }
        else
        {
            toolBar = true;
        }
    }

    void SpawnGroupOfObj()
    {
        FindClickPosition();
        int numOfObjects = 5;
        float spawnRange = 15.0f;
        float randX;
        float randZ;
        if (mana >= numOfObjects)
        {
            mana -= numOfObjects;
            if (currentTab == 0)
            {
                for (int i = 0; i < numOfObjects; i++)
                {
                    randX = Random.Range(-spawnRange, spawnRange + 1);
                    randZ = Random.Range(-spawnRange, spawnRange + 1);
                    Vector3 SpawnPos = new Vector3(objPos.x + randX, objPos.y, objPos.z + randZ);
                    SpawnObj(SpawnPos);
                }
            }
            else
            {
                Vector3 SpawnPos = new Vector3(objPos.x, objPos.y, objPos.z);
                SpawnPos.y = objToSpawn.transform.lossyScale.y / 2;
                SpawnObj(SpawnPos);
            }
        }
        else
        {
            Debug.Log("Insufficient Mana");
        }
    }

    void FindClickPosition()
    {
        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Ray ray = this.gameObject.GetComponent<Camera>().ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            objPos = hit.point;
            Debug.Log(hit.collider.gameObject);
        }
        else
        {
            objPos = this.gameObject.GetComponent<Camera>().ScreenToWorldPoint(mousePos);
        }
    }

    void SpawnObj(Vector3 randomPos)
    {
        if (currentTab == 0)
        {
            randomPos.y = objToSpawn.transform.lossyScale.y / 2;
            GameObject newObj = Instantiate(objToSpawn, randomPos, Quaternion.identity);
            newObj.transform.SetParent(GameObject.Find(objToSpawn.name + "s").transform);
            newObj.name = objToSpawn.name;
        }
        else
        {
            GameObject newObj = Instantiate(objToSpawn, randomPos, Quaternion.identity);
            newObj.name = objToSpawn.name;
        }
    }

    void UpdateUI()
    {
        manaText = GameObject.Find("ManaText");
        manaText.GetComponent<Text>().text = "Mana: " + mana;
        highlight.transform.position = selectPowerImg.transform.position;

        Image[] children = tabs[currentTab].GetComponentsInChildren<Image>();
        foreach (Image tempChild in children)
        {
            tempChild.enabled = true;
        }
        for (int i = 0; i < numOfTabs; i++)
        {
            if(i != currentTab)
            {
                Image[] children2 = tabs[i].GetComponentsInChildren<Image>();
                foreach (Image tempChild in children2)
                {
                    tempChild.enabled = false;
                }

            }
        }
    }

    void UpdateMana()
    {
        if (runningTimer == false)
        {
            StartCoroutine(timer(30.0f, JimManaFlow));
        }
    }

    void JimManaFlow()
    {
        ListOfObjects objList = GameObject.Find("ObjectLists").GetComponent<ListOfObjects>();
        mana += (objList.jimList.Length * 0.25f);
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time);
        funcToRun();
        runningTimer = false;
    }
}