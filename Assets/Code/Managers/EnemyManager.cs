using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DON'T TOUCH! The spawn system
/// </summary>
public class EnemyManager : MonoBehaviour
{
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

    #region Singleton
    public static EnemyManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<SpawnSet> spawnSet;//Holds the groups to be spawned
    public List<EnemyPool> enemyPool;//Holds the enemies for re-use
    public List<GameObject> enemyCompareList;//One of each type of enemy to use for comparison purposes
    public Dictionary<string, GameObject> enemyPrefabList = new Dictionary<string, GameObject>();
    //Guessing you could instantiate an object to get it's name and then place the string and prefab in the dictionary, then delete the compare list

    public void Start()
    {
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
}