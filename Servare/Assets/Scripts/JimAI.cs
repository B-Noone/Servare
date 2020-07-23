using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JimAI : MonoBehaviour {

    public int timeMultiplier = 1;
    public float hunger = 75.0f;
    private float maxHunger = 100.0f;
    public float health = 75.0f;
    private float maxHealth = 100.0f;

    public float interationRange = 1.0f;
    public float moveSpeed = 15.0f;

    public bool moving = false; //Checks if AI is moving
    //bool cannibal = false;
    public GameObject Objective = null; //The position of the Object the AI is going for
    string objFind = null;
    private GameObject objObjectList;
    private ListOfObjects objectList;
    public enum action {Nothing, Build, Gather, Collect};
    public action currentAction = action.Nothing;
    public enum resourceSearch {Nothing, Wood, Food, Stone};
    public resourceSearch currentSearch = resourceSearch.Nothing;
    public resourceSearch inventory = resourceSearch.Nothing;
    public GameObject StockpileObj;
    public Stockpile StockpileScript;
    public GameObject BuildingToConstruct;
    public BuildingConstruct BuildingRequirements;
    public bool builtSomething = false;
    public UnitV2 thisUnit;


    private bool runningTimer = false; //bool to run timer

	void Start () {
        objObjectList = GameObject.Find("ObjectLists");
        objectList = objObjectList.GetComponent<ListOfObjects>();
        StockpileScript = StockpileObj.GetComponent<Stockpile>();
        thisUnit = gameObject.GetComponent<UnitV2>();
    }
	
	void Update () {
        //if (currentAction == action.Collect && currentSearch == resourceSearch.Food)
        //    Debug.Log("THIS THIS THIS HTIS HITS");
        MainAIControl(); //Controls AI motives and when to move
        Needs(); //Controls needs degredation e.g. Hunger
        Checks();
        ErrorCheck();
        if (Objective == null)
        {
            //Debug.Log("New Objective");
            FindObjectName(objFind);
        }
    }

    void MainAIControl()
    {
        if (moving == false)
        {
            if (hunger < 50) // Find food if Hungry
            {
                if(inventory == resourceSearch.Food) {
                    Eat();
                }
                else if (StockpileScript.food == 0)
                {
                    objFind = "Bush";
                    currentAction = action.Gather;
                    currentSearch = resourceSearch.Food;
                    moving = true;
                }
                else
                {
                    currentAction = action.Collect;
                    currentSearch = resourceSearch.Food;
                    MoveToClosestStockpile();
                    moving = true;
                }
            }
            else if(BuildingToConstruct == null)
            {
                SetConstructionBuilding(FindConstruction());
                if(BuildingToConstruct == null)
                {
                    Objective = StockpileObj;
                    GatherStock();
                    moving = true;
                }
            }
            else if(BuildingToConstruct != null)
            {
                BuildConstruct();
                moving = true;
            }
            //else
            //{
            //    currentAction = action.Nothing;
            //    currentSearch = resourceSearch.Nothing;
            //}
        }
        if (moving == true)
        {
            if (Objective != null) //Checks there is objects to find
            {
                //MoveToObject(); //moves to Objective
                if (Destination()) //Checks if AI has reached Objective
                {
                    Interact();
                    moving = false;
                }
            }
        }
    }

    void Checks()
    {
        if(hunger > maxHunger)
        {
            hunger = maxHunger;
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        //if(cannibal == true)
        //{
        //    objFind = "Jim";
        //}
        if (Objective != null && thisUnit.target != Objective)
        {
            thisUnit.target = Objective.transform;
            thisUnit.Start();
        }
    }

    void FindObjectName(string type)
    {
        if (type != "null")
        {
            if (type == "Bush")
            {
                FindClosestObject(objectList.bushList);
            }
            else if (type == "Rock")
            {
                FindClosestObject(objectList.rockList);
            }
            else if (type == "Tree")
            {
                FindClosestObject(objectList.treeList);
            }
        }
    }

    void FindClosestObject(GameObject[] tempList)
    {
        if (tempList.Length >= 1)
        {
            Objective = tempList[0];
            for (int i = 0; i < tempList.Length; i++)
            {
                if (tempList[i] != null)
                {
                    //Debug.Log(objectList.bushList[i]);
                    if (Objective != null && (DistanceDifference(tempList[i].transform.position).magnitude < DistanceDifference(Objective.transform.position).magnitude) && (Objective != gameObject))
                    {
                        if (tempList[i] != null)
                        {
                            Objective = tempList[i];
                        }
                    }
                }
            }
        }
    }

    void MoveToObject() //Simple move towards
    {
        if (Objective != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Objective.transform.position, Time.deltaTime * moveSpeed);
            gameObject.transform.LookAt(Objective.transform);
        }
    }

    bool Destination() //Check if AI is at Objective
    {
        if (((transform.position.x <= Objective.transform.position.x + interationRange) && (transform.position.x >= Objective.transform.position.x - interationRange)) && ((transform.position.z <= Objective.transform.position.z + interationRange) && (transform.position.z >= Objective.transform.position.z - interationRange)))
        {
            return true;
        }
        return false;
    }

    void Needs() //Controls AI needs
    {
        if(runningTimer == false)
        {
            StartCoroutine(timer(10.0f, CoroutineProcesses));
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void Split()
    {
        GameObject newJim = Instantiate(gameObject);
        JimAI script = newJim.GetComponent<JimAI>();
        script.hunger = Random.Range(20, 60);
        script.health = maxHealth/2;
        script.Objective = null;
        newJim.name = "Jim";
        newJim.transform.SetParent(objObjectList.transform);
        newJim.transform.position = gameObject.transform.position;
        health = maxHealth/2;
        //hunger = Random.Range(20, 60);
    }

    void Starve()
    {
        health -= 5;
    }

    void ReduceHunger()
    {
        hunger -= 2f;
        if(hunger < 0)
        {
            hunger = 0;
        }
    }

    void HungerHeal()
    {
        hunger -= 5.0f;
        health += 5.0f;
    }

    void CoroutineProcesses()
    {
        //Debug.Log("Test Coroutines");
        if (hunger > 0)
        {
            //StartCoroutine(timer(2.0f, ReduceHunger));
            ReduceHunger();
        }
        if (hunger == 0)
        {
            //StartCoroutine(timer(2.0f, Starve));
            Starve();
        }
        if (health == maxHealth)
        {
            //StartCoroutine(timer(2.0f, Split));
            Split();
        }
        if (hunger > 50 && health < maxHealth)
        {
            //StartCoroutine(timer(2.0f, HungerHeal));
            HungerHeal();
        }
    }

    Vector3 DistanceDifference(Vector3 currObj) //Find how far an object is
    {
        if (currObj != new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity))
        {
            Vector3 diff = (gameObject.transform.position - currObj);
            return diff;
        }
        Vector3 nullValueResult = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        return nullValueResult;
    }

    IEnumerator timer(float time, System.Action funcToRun)
    {
        runningTimer = true;
        yield return new WaitForSeconds(time/timeMultiplier);
        funcToRun();
        runningTimer = false;
    }

    Vector3 NullVector() //Random unlikely value to signal a Vector is empty
    {
        Vector3 temp = new Vector3(-999899, -999899, -999899);
        return temp;
    }

    void Interact()
    {
        
        if (currentAction == action.Gather)
        {
            //Debug.Log("Gather");
            if (Objective.name == "StockpileBuilding")
            {
                //Debug.Log("Interact with Building");
                if (inventory == resourceSearch.Food)
                {
                    StockpileScript.food++;
                }
                else if (inventory == resourceSearch.Wood)
                {
                    StockpileScript.wood++;
                }
                else if (inventory == resourceSearch.Stone)
                {
                    StockpileScript.stone++;
                }
                inventory = resourceSearch.Nothing;
            }
            else
            {
                ResourceControl RCScript = Objective.GetComponent<ResourceControl>();
                if (RCScript != null)
                {
                    RCScript.numberOfResource -= 1;
                    if (Objective.name == "Bush")
                    {
                        inventory = resourceSearch.Food;
                    }
                    else if (Objective.name == "Tree")
                    {
                        inventory = resourceSearch.Wood;
                    }
                    else if (Objective.name == "Rock")
                    {
                        inventory = resourceSearch.Stone;
                    }
                }
            }

            ResetVars();
        }
        if(currentAction == action.Collect)
        {
            //Debug.Log("Collect");
            //Stockpile StockScript = Objective.GetComponent<Stockpile>();
            if (currentSearch == resourceSearch.Food && StockpileScript.food != 0)
            {
                StockpileScript.food -= 1;
                inventory = resourceSearch.Food;
            }
            else if(currentSearch == resourceSearch.Wood && StockpileScript.wood != 0)
            {
                StockpileScript.wood -= 1;
                inventory = resourceSearch.Wood;
            }
            else if(currentSearch == resourceSearch.Stone && StockpileScript.stone != 0)
            {
                StockpileScript.stone -= 1;
                inventory = resourceSearch.Stone;
            }

        } 
        if (currentAction == action.Build)
        {
            BuildingConstruct BuildScript = Objective.GetComponent<BuildingConstruct>();
            if(inventory == resourceSearch.Wood)
            {
                if(BuildScript.wood > 0)
                {
                    BuildScript.wood -= 1;
                    inventory = resourceSearch.Nothing;
                    BuildingToConstruct = null;
                    BuildingRequirements = null;
                }
            }
            else if(inventory == resourceSearch.Stone)
            {
                if(BuildScript.stone > 0)
                {
                    BuildScript.stone -= 1;
                    inventory = resourceSearch.Nothing;
                    BuildingToConstruct = null;
                    BuildingRequirements = null;
                }
            }
            else
            {
                currentAction = action.Collect;
            }
        }
        //Debug.Log("Reset Vars");
        //ResetVars();
    }

    void Eat()
    {
        inventory = resourceSearch.Nothing;
        hunger += 20;
        if(hunger > maxHunger)
        {
            hunger = maxHunger;
        }
    }

    void SetConstructionBuilding(GameObject cObj)
    {
        BuildingToConstruct = cObj;
    }

    GameObject FindConstruction()
    {
        for (int i = 0; i < objectList.constructionList.Length; i++)
        {
            if (objectList.constructionList[i] != null)
            {
                return objectList.constructionList[i];
            }
        }
        return null;
    }

    void BuildConstruct()
    {
        //Debug.Log("Build Construct");
        BuildingRequirements = BuildingToConstruct.GetComponent<BuildingConstruct>();
        if (inventory != resourceSearch.Wood && inventory != resourceSearch.Stone)
        {
            if (BuildingRequirements.wood > 0)
            {
                //Debug.Log("Jim LOOKS FOR WOOD");
                if (StockpileScript.wood <= 0)
                {
                    //Debug.Log("Jim LOOKS FOR TREES");
                    //Debug.Log("Build Construct: Wood");
                    objFind = "Tree";
                    currentAction = action.Gather;
                    currentSearch = resourceSearch.Wood;
                }
                else if (StockpileScript.wood > 0)
                {
                    //Debug.Log("Jim LOOKS FOR WOODPILE");
                    currentAction = action.Collect;
                    currentSearch = resourceSearch.Wood;
                }
            }

            else if (BuildingRequirements.stone > 0)
            {
                if (StockpileScript.stone <= 0)
                {
                    //Debug.Log("Build Construct: Stone");
                    objFind = "Rock";
                    currentAction = action.Gather;
                    currentSearch = resourceSearch.Stone;
                }
                else if (StockpileScript.stone > 0)
                {
                    currentAction = action.Collect;
                    currentSearch = resourceSearch.Stone;
                }
            }
        }
        else if(BuildingToConstruct != null)
        {
            //Debug.Log("Build Construct: Building");
            currentAction = action.Build;
            Objective = BuildingToConstruct;
        }
        builtSomething = true;
    }

    void ResetVars()
    {
        currentAction = action.Nothing;
        currentSearch = resourceSearch.Nothing;
        objFind = "null";
        Objective = null;
        moving = false;
    }

    void ErrorCheck()
    {
        if ((builtSomething == true && moving == true) && (BuildingToConstruct == null || BuildingRequirements == null))
        {
            moving = false;
            BuildingToConstruct = null;
            builtSomething = false;
            BuildingRequirements = null;
        }
        if (Objective == null)
        {
            moving = false;
        }
    }

    void GatherStock()
    {
        if (inventory == resourceSearch.Nothing)
        {
            currentAction = action.Gather;
            FindClosestObject(objectList.resourceList);
        }
        if (inventory != resourceSearch.Nothing)
        {
            currentAction = action.Gather;
            MoveToClosestStockpile();
        }
    }

    void MoveToClosestStockpile()
    {
        int listNum = 0;
        GameObject[] tempArray;
        tempArray = new GameObject[objectList.buildingList.Length];
        for (int i = 0; i < objectList.buildingList.Length; i++)
        {
            if (objectList.buildingList[i].name == "StockpileBuilding")
            {
                tempArray[listNum] = objectList.buildingList[i];
                listNum++;
            }
        }
        FindClosestObject(tempArray);

        //MoveToObject();
    }
}