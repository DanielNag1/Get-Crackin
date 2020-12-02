using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public List<positionSquaresData> positionSquaresUsed;
    #region SpawnSystem
    // DON'T TOUCH! The spawn system
    /// <summary>
    /// To configuate which type of pools we want from the inspecter
    /// </summary>
    [System.Serializable]
    public class SpawnSet
    {
        public List<GameObject> spawnPoints;
        public List<string> spawnPrefabName;
    }


    //Data container
    public class EnemyPool
    {
        public GameObject enemy;
        public bool elementAvailable = false;
        public EnemyPool(GameObject enemy, bool elementAvailable)
        {
            this.enemy = enemy;
            this.elementAvailable = elementAvailable;
        }
    }


    public List<SpawnSet> spawnSet;//Holds the groups to be spawned
    public List<EnemyPool> enemyPool;//Holds the enemies for re-use
    public List<GameObject> enemyCompareList;//One of each type of enemy to use for comparison purposes
    public Dictionary<string, GameObject> enemyPrefabList = new Dictionary<string, GameObject>();
    //Guessing you could instantiate an object to get it's name and then place the string and prefab in the dictionary, then delete the compare list
    public void Start()
    {
        positionSquaresUsed = new List<positionSquaresData>();
        Transform tempTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("NormalizedCirclePosition");
        for (int i = 0; i < 12; i++)
        {
            positionSquaresUsed.Add(new positionSquaresData(tempTransform.Find("Position " + i), i, true));
        }

        enemyPool = new List<EnemyPool>();
        for (int i = 0; i < enemyCompareList.Count; i++)
        {
            GameObject prefabName = Instantiate(enemyCompareList[i]);
            string temp = prefabName.name;
            enemyPrefabList.Add(temp, enemyCompareList[i]); //unsure how changing temp to prefabName.name is acctually going to be handled.
            Destroy(prefabName);
        }
        enemyCompareList = null;
    }

    /// <summary>
    /// Spawns enemies from a specified spawnSet, only creates new enemies if there are none of the required enemy type available.
    /// </summary>
    /// <param name="setToUse">The element corresponding to the set we want.</param>
    public void SpawnEnemyFromTrigger(int setToUse)
    {
        //we have a list of enemies we want to spawn.
        //spawnSet[setToUse].spawnPrefab

        //Search the enemyPool for an available enemy for every enemy in the seleted spawnSet.
        for (int i = 0; spawnSet[setToUse].spawnPrefabName.Count > i; ++i)
        {
            EnemyPool enemy = Find(spawnSet[setToUse].spawnPrefabName[i] + "(Clone)", enemyPool);//Find() returns default value hence we need a null check
            //if not found, create the enemy && add to pool && set unavailable in pool && spawn
            if (enemy == null)
            {
                //  + "(Clone)") is added because we took the name from an instanciated prefab.
                if (enemyPrefabList.ContainsKey(spawnSet[setToUse].spawnPrefabName[i] + "(Clone)"))
                {
                    enemyPool.Add(new EnemyPool(Instantiate(enemyPrefabList[spawnSet[setToUse].spawnPrefabName[i] + "(Clone)"]), false));
                    enemy = enemyPool[enemyPool.Count - 1];
                    for (int J = 0; J < enemy.enemy.transform.childCount; J++)
                    {
                        enemy.enemy.transform.GetChild(J).gameObject.SetActive(true);
                    }
                }
            }
            //else, update stats && set unavailable in pool && spawn
            else
            {
                for (int J = 0; J < enemy.enemy.transform.childCount; J++)//Loop thrue the entire game object and activate EVERYTHING!
                {
                    enemy.enemy.transform.GetChild(J).gameObject.SetActive(true);
                }
                //enemy.enemy.GetComponentInChildren<enemyhealth>().Reset();
                enemy.elementAvailable = false;
            }
            enemy.enemy.transform.position = spawnSet[setToUse].spawnPoints[i].transform.position;
            enemy.enemy.transform.rotation = spawnSet[setToUse].spawnPoints[i].transform.rotation;
            NavMeshHit hit;
            int yes = 1;
            while (!NavMesh.SamplePosition(spawnSet[setToUse].spawnPoints[i].transform.position, out hit, yes, NavMesh.AllAreas)) //get where we should go on the navMesh      
            {
                yes++;
            }
            enemy.enemy.GetComponentInChildren<NavMeshAgent>().Warp(hit.position);
        }
    }
    public EnemyPool Find(string prefabName, List<EnemyPool> enemyPool)
    {
        foreach (var enemy in enemyPool)
        {
            if (enemy.elementAvailable)//is the enemy currently active in the scene?
            {
                if (prefabName == enemy.enemy.name)
                    return enemy;
            }
        }
        return null;
    }
    #endregion

    #region CombatManager
    public class CombatAgentdata
    {
        public GameObject agentGameObject;
        public bool isReady;
        public CombatAgentdata(GameObject agentGameObject, bool ready)
        {
            isReady = ready;
            this.agentGameObject = agentGameObject;
        }
    }

    public class positionSquaresData
    {
        public Transform squareTransform;
        public bool isAvailable;
        public int id;
        public positionSquaresData(Transform squareTransform, int id, bool available)
        {
            isAvailable = available;
            this.squareTransform = squareTransform;
            this.id = id;
        }
    }

    public List<CombatAgentdata> agentsInCombat = new List<CombatAgentdata>();
    public List<CombatAgentdata> agentsReadyToAttack = new List<CombatAgentdata>();
    private float coolDownTime = 0.1f;
    private int meleeCombatants = 0;
    private int rangedComtabatants = 0;

    /// <summary>
    /// Alerts all close agents of the players presence
    /// </summary>
    /// <param name="detectingAgent"></param>
    /// <param name="foxAgentFSM"></param>
    public void AgentDetectedPlayer(GameObject detectingAgent, float talkingDistance)
    {
        //elementAvailable==false means the agent is active in the scene.
        //Get a list of all active agents within talkingDistance.
        var tempList = enemyPool.FindAll((x) => x.elementAvailable == false
        && Vector3.Distance(x.enemy.GetComponentInChildren<Transform>().position, detectingAgent.transform.position) < talkingDistance);
        foreach (var gameObject in tempList)
        {
            bool unique = true;
            for (int i = 0; i < agentsInCombat.Count; i++)
            {
                if (agentsInCombat[i].agentGameObject.GetInstanceID() == gameObject.enemy.GetInstanceID())
                {
                    unique = false;
                    break;
                }
            }
            if (unique)
            {
                agentsInCombat.Add(new CombatAgentdata(gameObject.enemy, false));
            }
        }
    }

    /// <summary>
    /// Removes an agent from the agentInCombat collection, returns true if removed
    /// </summary>
    /// <param name="foxAgentFSM"></param>
    /// <returns></returns>
    public bool AgentLeftCombat(GameObject foxAgentGameObject)
    {
        if (agentsInCombat.Exists((x) => x.agentGameObject.GetInstanceID() == foxAgentGameObject.transform.root.gameObject.GetInstanceID()))
        {
            switch (foxAgentGameObject.GetComponent<FoxAgentFSM>().combatRole)
            {
                case FoxAgentFSM.CombatRole.None:
                    break;
                case FoxAgentFSM.CombatRole.Melee:
                    meleeCombatants--;
                    break;
                case FoxAgentFSM.CombatRole.Ranged:
                    rangedComtabatants--;
                    break;
            }
            foxAgentGameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.None;
            positionSquaresUsed[foxAgentGameObject.GetComponent<FoxAgentFSM>().squareID].isAvailable = true;
            agentsInCombat.Remove(agentsInCombat.Find((x) => x.agentGameObject.GetInstanceID() == foxAgentGameObject.transform.root.gameObject.GetInstanceID()));
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (IssueAttackOrderToReadyAgent())
        {
            coolDownTime = UnityEngine.Random.Range(2, 5);
        }
    }

    /// <summary>
    /// Attempts to find an agentInCombat that is able to attack, Then at random tells one of the agents to start an attack.
    /// returns true if an attack order was sent.
    /// </summary>
    public bool IssueAttackOrderToReadyAgent()
    {
        var temp = agentsInCombat.FindAll((x) => x.isReady == true);
        if (temp.Count != 0)
        {
            coolDownTime -= Time.deltaTime;
            if (coolDownTime < 0)
            {
                temp[UnityEngine.Random.Range(0, agentsReadyToAttack.Count)].agentGameObject.GetComponentInChildren<FoxAgentFSM>().attacking = true;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Sets the readyToAttack bool in the agentsInCombat collection to readyToAttack, if the agent is not in the collection we add it.
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="readyToAttack"></param>
    public void SetReadyToAttack(GameObject agent, bool readyToAttack)
    {
        if (agentsInCombat.Exists((x) => x.agentGameObject.GetInstanceID() == agent.transform.root.gameObject.GetInstanceID()))
        {
            agentsInCombat.Find((x) => x.agentGameObject.GetInstanceID() == agent.transform.root.gameObject.GetInstanceID()).isReady = readyToAttack;
        }
        else
        {
            Debug.Log("All Agents should be in the list allready. If you read this something went wrong!");
            agentsInCombat.Add(new CombatAgentdata(agent.transform.root.gameObject, readyToAttack));
        }
    }

    public void AssignCombatRoleAndCircleRadius(GameObject gameObject)
    {
        foreach (var item in agentsInCombat)
        {
            if (meleeCombatants / 2 > rangedComtabatants)
            {

                rangedComtabatants++;
                item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Ranged;
                item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius = 8;
            }
            else
            {
                meleeCombatants++;
                item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Melee;
                item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius = 4;
            }
        }
    }

    public void SetCombatRole(GameObject gameObject, FoxAgentFSM.CombatRole combatRole)
    {
        switch (gameObject.GetComponent<FoxAgentFSM>().combatRole)
        {
            case FoxAgentFSM.CombatRole.None:
                break;
            case FoxAgentFSM.CombatRole.Melee:
                meleeCombatants--;
                break;
            case FoxAgentFSM.CombatRole.Ranged:
                rangedComtabatants--;
                break;
        }
        if (combatRole == FoxAgentFSM.CombatRole.Melee)
        {
            rangedComtabatants++;
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Ranged;
            gameObject.GetComponent<FoxAgentFSM>().circleRadius = 8;
        }
        else if (combatRole == FoxAgentFSM.CombatRole.Melee)
        {
            meleeCombatants++;
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Melee;
            gameObject.GetComponent<FoxAgentFSM>().circleRadius = 4;
        }
        else if (combatRole == FoxAgentFSM.CombatRole.None)
        {
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.None;
        }
    }
    public void AssignSquare(GameObject agent)
    {
        int tempID = positionSquaresUsed.Find((x) => x.isAvailable == true).id;
        agent.GetComponent<FoxAgentFSM>().squareID = positionSquaresUsed[tempID].id;
        agent.GetComponent<FoxAgentFSM>().squareNormalisedPosition = positionSquaresUsed[tempID].squareTransform.localPosition;
        positionSquaresUsed[tempID].isAvailable = false;
    }

    public void SteeringBehaviorDestinationUpdate()
    {
        //List<GameObject> agent;
        //Vector3 agentDirection;
        //float agentCircleRadius;
        //Vector3 agentPositionOnCircle;
        //Vector3 desiredMinimumDistanceBetweenAgents = 1.5;
        //after calculating the position on the circle we can add separation

        //List<GameObject> agentsWithAgentPositionOnCircleCloseEnoughToInfluenceMe;
        //Vector3 

    }

    /// <summary>
    /// Calculates and returns the direction towards a perticular GameObject
    /// </summary>
    public void Approach(GameObject agent, GameObject targetToApprach)
    {
        Vector3 steeringVector = Vector3.zero;
        //Start-End gives direction from where we are to where we want to go.
        steeringVector = agentsInCombat[0].agentGameObject.GetComponentInChildren<Transform>().position - targetToApprach.transform.position;
        steeringVector.Normalize();
    }

    /// <summary>
    /// Calculates and returns the direction towards a perticular point
    /// </summary>
    public Vector3 Approach(GameObject agent, Vector3 targetToApprach)
    {
        Vector3 steeringVector = Vector3.zero;
        //Start-End gives direction from where we are to where we want to go.
        steeringVector = agentsInCombat[0].agentGameObject.GetComponentInChildren<Transform>().position - targetToApprach;
        steeringVector.Normalize();
        return steeringVector;
    }

    public void Evade()
    {
        ////get where others are on circle, 
        //for (int i = 0; i < agentsInCombat.Count; i++)
        //{
        //    for (int j = i + 1; j < agentsInCombat.Count; j++)
        //    {
        //        if (i == j)
        //        {
        //            continue;
        //        }
        //        //Possible problem with agents allways moving to the right of the eachother ???
        //        if (Vector3.Distance(
        //            agentsInCombat[i].agentGameObject.GetComponentInChildren<NavMeshAgent>().destination,
        //            agentsInCombat[j].agentGameObject.GetComponentInChildren<NavMeshAgent>().destination) < 2/*Minimum distance before corrections*/)
        //        {
        //            Debug.Log("ToClose!");
        //            double alpha = Math.Asin(3/*desired distance*// (2 * agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius)) * 2;
        //            NavMeshHit hit;
        //            int k = 1;
        //            while (!NavMesh.SamplePosition(new Vector3((float)Math.Cos(alpha), 0, (float)Math.Sin(alpha)) * agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius + agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().player.transform.position, out hit, k, NavMesh.AllAreas)) //get where we should go on the navMesh
        //            {
        //                k++;
        //            }
        //            agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().destination = hit.position; //set as target position.
        //            Debug.Log(agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().destination);
        //            agentsInCombat[j].agentGameObject.GetComponentInChildren<NavMeshAgent>().destination = agentsInCombat[j].agentGameObject.GetComponentInChildren<FoxAgentFSM>().destination;
        //        }
        //    }
        //}
    }

    public void Evade(GameObject agentGameObject)
    {
        #region boids
        //Vector3 resVec = Vector3.zero;
        //var neighborCount = 0;
        //foreach (var item in agentsInCombat)
        //{
        //    if (item.agentGameObject.GetInstanceID() == agentGameObject.transform.root.gameObject.GetInstanceID())
        //    {
        //        continue;
        //    }
        //    if (Vector3.Distance(item.agentGameObject.GetComponentInChildren<NavMeshAgent>().destination,
        //        agentGameObject.GetComponent<NavMeshAgent>().destination) < 2/*Minimum distance before corrections*/)
        //    {
        //        resVec.x += item.agentGameObject.GetComponentInChildren<NavMeshAgent>().transform.position.x - agentGameObject.transform.position.x;
        //        resVec.z += item.agentGameObject.GetComponentInChildren<NavMeshAgent>().transform.position.z - agentGameObject.transform.position.y;
        //        neighborCount++;
        //    }
        //}

        //if (neighborCount == 0)
        //    return resVec;
        //resVec.x /= neighborCount;
        //resVec.z /= neighborCount;
        //resVec.x *= -1;
        //resVec.z *= -1;
        //resVec.Normalize();
        //return resVec;
        #endregion

        #region MathRadius
        ////get where others are on circle, 
        //int i = 0;
        //while (i < agentsInCombat.Count)
        ////for (int i = 0; i < agentsInCombat.Count; i++)
        //{
        //    if (agentsInCombat[i].agentGameObject.GetInstanceID() == agentGameObject.transform.root.gameObject.GetInstanceID())
        //    {
        //        continue;
        //    }

        //    //Possible problem with agents allways moving to the right of the eachother ???
        //    if (Vector3.Distance(
        //        agentsInCombat[i].agentGameObject.GetComponentInChildren<NavMeshAgent>().destination,
        //        agentGameObject.GetComponent<NavMeshAgent>().destination) < 2/*Minimum distance before corrections*/)
        //    {
        //        Debug.Log("ToClose!");
        //        Vector3 playerToAgent = agentGameObject.GetComponent<FoxAgentFSM>().player.transform.position - agentsInCombat[i].agentGameObject.GetComponentInChildren<NavMeshAgent>().destination;
        //        playerToAgent.y = 0;
        //        playerToAgent.Normalize();
        //        double degrees = (180 / Math.PI) * Math.Atan2(playerToAgent.y, playerToAgent.x);
        //        Debug.Log("degrees=" + degrees);
        //        double alpha = Math.Asin(3/*desired distance*// (2 * agentGameObject.GetComponent<FoxAgentFSM>().circleRadius)) * 2;
        //        Debug.Log("alpha=" + alpha);
        //        NavMeshHit hit;
        //        int k = 1;
        //        while (!NavMesh.SamplePosition(new Vector3((float)Math.Cos(alpha + degrees), 0, (float)Math.Sin(alpha + degrees)) * agentGameObject.GetComponent<FoxAgentFSM>().circleRadius + agentGameObject.GetComponent<FoxAgentFSM>().player.transform.position, out hit, k, NavMesh.AllAreas)) //get where we should go on the navMesh
        //        {
        //            k++;
        //        }
        //        agentGameObject.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        //        Debug.Log("New destination" + agentGameObject.GetComponent<FoxAgentFSM>().destination);
        //        agentGameObject.GetComponent<NavMeshAgent>().destination = agentGameObject.GetComponent<FoxAgentFSM>().destination;
        //        i = 0;
        //    }
        //    else
        //    {
        //        i++;
        //    }

        //}
        #endregion

        #region MathMoveRadius
        //int angleBetweenAgents = 360 / agentsInCombat.Count;
        //int i = 0;
        //foreach (var agent in agentsInCombat)
        //{
        //    NavMeshHit hit;
        //    int k = 1;
        //    while (!NavMesh.SamplePosition((new Vector3((float)Math.Cos(angleBetweenAgents*i), 0, (float)Math.Sin(angleBetweenAgents * i)) * agentGameObject.GetComponent<FoxAgentFSM>().circleRadius) + agentGameObject.GetComponent<FoxAgentFSM>().player.transform.position, out hit, k, NavMesh.AllAreas)) //get where we should go on the navMesh
        //    {
        //        k++;
        //    }
        //    agent.agentGameObject.GetComponentInChildren<FoxAgentFSM>().destination = hit.position; //set as target position.
        //    agent.agentGameObject.GetComponentInChildren<NavMeshAgent>().destination = agent.agentGameObject.GetComponentInChildren<FoxAgentFSM>().destination;
        //}
        #endregion

        #region AssignAvailableSquare

        #endregion
    }
}
#endregion