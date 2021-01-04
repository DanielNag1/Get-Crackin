using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

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
    #region Variables
    public List<SpawnSet> spawnSet;//Holds the groups to be spawned
    public List<EnemyPool> enemyPool;//Holds the enemies for re-use
    public List<GameObject> enemyCompareList;//One of each type of enemy to use for comparison purposes
    public Dictionary<string, GameObject> enemyPrefabList = new Dictionary<string, GameObject>();
    #endregion
    //Guessing you could instantiate an object to get it's name and then place the string and prefab in the dictionary, then delete the compare list
    public void Start()
    {
        #region PlayerSquares
        positionSquaresUsed = new List<positionSquaresData>();
        Transform tempTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("NormalizedCirclePosition");
        for (int i = 0; i < 12; i++)
        {
            positionSquaresUsed.Add(new positionSquaresData(tempTransform.Find("Position " + i), i, true));
        }
        #endregion

        enemyPool = new List<EnemyPool>();
        for (int i = 0; i < enemyCompareList.Count; i++)
        {
            GameObject prefabName = Instantiate(enemyCompareList[i], transform.position, Quaternion.identity);
            string temp = prefabName.name;
            enemyPrefabList.Add(temp, enemyCompareList[i]); //unsure how changing temp to prefabName.name is acctually going to be handled.
            Destroy(prefabName);
        }
        enemyCompareList = null;

        //init of combat only enemy prefabs will occupy a square on their initialisation.
        foreach (var item in positionSquaresUsed)
        {
            item.isAvailable = true;
        }

        PopulateReloadStationList();
    }

    public void KillCurrentEnemies()
    {
        foreach (var enemy in enemyPool)
        {
            enemy.enemy.GetComponentInChildren<EnemyHealth>().KillCurrentEnemy();
        }
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
                    enemyPool.Add(new EnemyPool(Instantiate(enemyPrefabList[spawnSet[setToUse].spawnPrefabName[i] + "(Clone)"],
                        transform.position, Quaternion.identity), false));
                    enemy = enemyPool[enemyPool.Count - 1];
                    for (int j = 0; j < enemy.enemy.transform.childCount; j++)
                    {
                        enemy.enemy.transform.GetChild(j).gameObject.SetActive(true);
                    }
                }
            }
            //else, update stats && set unavailable in pool && spawn
            else
            {
                for (int j = 0; j < enemy.enemy.transform.childCount; j++)//Loop thrue the entire game object and activate EVERYTHING!
                {
                    enemy.enemy.transform.GetChild(j).gameObject.SetActive(true);
                }
                //enemy.enemy.GetComponentInChildren<enemyhealth>().Reset();
                enemy.elementAvailable = false;
            }
            enemy.enemy.transform.position = spawnSet[setToUse].spawnPoints[i].transform.position;
            enemy.enemy.transform.rotation = spawnSet[setToUse].spawnPoints[i].transform.rotation;
            NavMeshHit hit;
            int sampleDistance = 1;
            while (!NavMesh.SamplePosition(spawnSet[setToUse].spawnPoints[i].transform.position, out hit, sampleDistance, NavMesh.AllAreas)) //get where we should go on the navMesh      
            {
                sampleDistance++;
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
            this.isReady = ready;
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
            this.isAvailable = available;
            this.squareTransform = squareTransform;
            this.id = id;
        }
    }

    #region variables
    [SerializeField] private TriggerComponent _triggerComponent;
    public List<CombatAgentdata> agentsInCombat = new List<CombatAgentdata>();
    public List<CombatAgentdata> agentsReadyToAttack = new List<CombatAgentdata>();
    public List<Transform> reloadStations = new List<Transform>();
    [SerializeField] private GameObject reloadStationFolderObject;
    private float _coolDownTime = 0.1f;
    private int _meleeCombatants = 0;
    private int _rangedComtabatants = 0;
    [SerializeField] private float meleeCircleRadius = 4;
    [SerializeField] private float rangedCircleRadius = 8;
    private bool _inArenaFight = false;
    #endregion

    public void StartArenaFight(List<string> callList, List<int> setToUse, List<GameObject> objectToMove)
    {
        _inArenaFight = true;
        int BreakElement = callList.FindIndex(x => x == "Break");
        if (BreakElement != -1)
        {
            //if last element is break we wount get index out of bounds.
            if (callList.Count >= BreakElement + 1)
            {
                _triggerComponent.ArenaFinishedClassesAndMethodsToBeCalled = callList.GetRange(BreakElement + 1, callList.Count - BreakElement - 1);
                callList.RemoveRange(BreakElement, callList.Count - BreakElement);
            }
        }
        _triggerComponent.ClassesAndMethodsToBeCalled = callList;
        _triggerComponent.SetToUse = setToUse;
        _triggerComponent.ObjectToMove = objectToMove;
    }

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
                    --_meleeCombatants;
                    break;
                case FoxAgentFSM.CombatRole.Ranged:
                    --_rangedComtabatants;
                    break;
            }
            foxAgentGameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.None;
            foxAgentGameObject.GetComponent<FoxAgentFSM>().SetFSMState(foxAgentGameObject.GetComponent<FoxAgentFSM>().defaultState);
            positionSquaresUsed[foxAgentGameObject.GetComponent<FoxAgentFSM>().squareID].isAvailable = true;
            agentsInCombat.Remove(agentsInCombat.Find((x) => x.agentGameObject.GetInstanceID() ==
                foxAgentGameObject.transform.root.gameObject.GetInstanceID()));
            if (_inArenaFight && agentsInCombat.Count <= 0)
            {
                _inArenaFight = false;
                _triggerComponent.ActivateTrigger();
            }
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (IssueAttackOrderToReadyAgent())
        {
            _coolDownTime = UnityEngine.Random.Range(0, 2);
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
            _coolDownTime -= Time.deltaTime;
            if (_coolDownTime < 0)
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
            agentsInCombat.Find((x) => x.agentGameObject.GetInstanceID() == agent.transform.root.gameObject.GetInstanceID()).isReady =
                readyToAttack;
        }
        else
        {
            Debug.Log("All Agents should be in the list already. If you read this something went wrong!");
            agentsInCombat.Add(new CombatAgentdata(agent.transform.root.gameObject, readyToAttack));
        }
    }

    public void AssignCombatRoleAndCircleRadius()
    {
        foreach (var item in agentsInCombat)
        {
            if (item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().combatRole == FoxAgentFSM.CombatRole.None)
            {

                if (_meleeCombatants - _rangedComtabatants >= 2)
                {
                    //We only need to calculate temp if the agent could become a rangedCombatant.
                    //can be imporved by either making it only save the bool, or assigning the position to the object.
                    var temp = ClosestReloadStation(item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().transform, item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().maxSearchDistance);
                    if (temp.Item2)
                    {
                        AssignRangedCombatRole(item);
                        Debug.Log(item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().GetInstanceID() + " Assigned Ranged:" + _rangedComtabatants);
                        continue;
                    }
                }
                AssignMeleeCombatRole(item);
                Debug.Log(item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().GetInstanceID() + " Assigned Melee:" + _meleeCombatants);
            }
        }
    }

    public void AssignMeleeCombatRole(CombatAgentdata item)
    {
        ++_meleeCombatants;
        item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Melee;
        item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius = meleeCircleRadius;
    }

    public void AssignRangedCombatRole(CombatAgentdata item)
    {
        ++_rangedComtabatants;
        item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Ranged;
        item.agentGameObject.GetComponentInChildren<FoxAgentFSM>().circleRadius = rangedCircleRadius;
    }

    public void SetCombatRole(GameObject gameObject, FoxAgentFSM.CombatRole combatRole)
    {
        switch (gameObject.GetComponent<FoxAgentFSM>().combatRole)
        {
            case FoxAgentFSM.CombatRole.None:
                break;
            case FoxAgentFSM.CombatRole.Melee:
                --_meleeCombatants;
                break;
            case FoxAgentFSM.CombatRole.Ranged:
                --_rangedComtabatants;
                break;
        }
        if (combatRole == FoxAgentFSM.CombatRole.Melee)
        {
            ++_rangedComtabatants;
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Ranged;
            gameObject.GetComponent<FoxAgentFSM>().circleRadius = rangedCircleRadius;
        }
        else if (combatRole == FoxAgentFSM.CombatRole.Melee)
        {
            ++_meleeCombatants;
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.Melee;
            gameObject.GetComponent<FoxAgentFSM>().circleRadius = meleeCircleRadius;
        }
        else if (combatRole == FoxAgentFSM.CombatRole.None)
        {
            gameObject.GetComponent<FoxAgentFSM>().combatRole = FoxAgentFSM.CombatRole.None;
        }
    }

    public bool AssignSquare(GameObject agent)
    {
        var temp = positionSquaresUsed.FindAll((x) => x.isAvailable == true);
        int SquareID = -1;
        float distance = float.MaxValue;
        for (int element = 0; element < temp.Count; element++)
        {
            var calculatedDistance = Vector3.Distance(agent.transform.position, positionSquaresUsed[element].squareTransform.position);
            if (calculatedDistance < distance)
            {
                distance = calculatedDistance;
                SquareID = temp[element].id;
            }
        }
        if (SquareID != -1)
        {
            Debug.Log(agent.GetComponent<FoxAgentFSM>().GetInstanceID() + " Assigned Square:" + SquareID);
            positionSquaresUsed[SquareID].isAvailable = false;
            agent.GetComponent<FoxAgentFSM>().squareID = positionSquaresUsed[SquareID].id;
            agent.GetComponent<FoxAgentFSM>().squareNormalisedPosition = positionSquaresUsed[SquareID].squareTransform.localPosition;
            return true;
        }
        return false;
    }

    public bool UpdateAssignedSquare(GameObject agent)
    {
        positionSquaresUsed[agent.GetComponent<FoxAgentFSM>().squareID].isAvailable = true;
        var temp = positionSquaresUsed.FindAll((x) => x.isAvailable == true);
        int SquareID = -1;
        float distance = float.MaxValue;
        for (int element = 0; element < temp.Count; element++)
        {
            var calculatedDistance = Vector3.Distance(agent.transform.position, positionSquaresUsed[element].squareTransform.position);
            if (calculatedDistance < distance)
            {
                distance = calculatedDistance;
                SquareID = temp[element].id;
            }
        }
        if (SquareID != -1)
        {
            Debug.Log(agent.GetComponent<FoxAgentFSM>().GetInstanceID() + " Assigned Square:" + SquareID + ", Previous square:" + agent.GetComponent<FoxAgentFSM>().squareID);
            positionSquaresUsed[SquareID].isAvailable = false;
            agent.GetComponent<FoxAgentFSM>().squareID = positionSquaresUsed[SquareID].id;
            agent.GetComponent<FoxAgentFSM>().squareNormalisedPosition = positionSquaresUsed[SquareID].squareTransform.localPosition;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Create a list of all reloadStations under the folder object in the scene
    /// </summary>
    public void PopulateReloadStationList()
    {
        reloadStations = reloadStationFolderObject.transform.Cast<Transform>().ToList();
        reloadStationFolderObject = null;
    }

    public Tuple<Vector3, bool> ClosestReloadStation(Transform agentTransform, float maxSearchDistance)
    {
        Transform temp = agentTransform;
        float maxDistance = float.MaxValue;
        foreach (Transform reloadStationTransform in reloadStations)
        {
            float calcDistance = Vector3.Distance(agentTransform.position, reloadStationTransform.position);
            if (calcDistance < maxDistance && calcDistance < maxSearchDistance)
            {
                maxDistance = calcDistance;
                temp = reloadStationTransform;
            }
        }

        if (temp == agentTransform)
        {
            return new Tuple<Vector3, bool>(temp.position, false);
        }
        else
        {
            return new Tuple<Vector3, bool>(temp.position, true);
        }
    }
    #endregion
}