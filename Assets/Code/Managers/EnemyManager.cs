using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// To configuate which type of pools we want from the inspecter
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject enemyPrefab;
        public int totalSizeOfPool;  //At which point are we going to start reuse objects instead of spawning a new one.
    }

    #region Singleton
    public static EnemyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> poolList;

    /// <summary>
    /// Key (string) is the tag we assosioate each pool with.
    /// Second is the actual pool stored as a Queue of Gameobjects.
    /// </summary>
    public Dictionary<string, Queue<GameObject>> enemyPoolDictionary;

    void Start()
    {
        enemyPoolDictionary = new Dictionary<string, Queue<GameObject>>();  //Empty Dictionary.

        foreach (Pool pool in poolList)
        {
            Queue<GameObject> enemyPool = new Queue<GameObject>();  //A Queue of objects.

            //Create each one of these objects.
            for (int i = 0; i < pool.totalSizeOfPool; i++)
            {
                GameObject enemy = Instantiate(pool.enemyPrefab); 
                enemy.SetActive(false);  //We cant see it just yet!
                enemyPool.Enqueue(enemy);  //Add it to the end of the Queue and feed it the enemyobject.
            }

            enemyPoolDictionary.Add(pool.tag, enemyPool);  //Adding pool to Dictionary.
        }
    }

    /// <summary>
    /// Take inactive gameobjects and spawn them in to our active level.
    /// <paramref name="tag"/> = tag of the object we want to spawn.
    /// <paramref name="position"/> = where we want to spawn it
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnEnemyFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        //Safety first!!
        if(!enemyPoolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + " pool does not exist");
            return null;
        }

        GameObject enemyToSpawn = enemyPoolDictionary[tag].Dequeue(); //Dequeue to pull out the first element in the queue.
        enemyToSpawn.SetActive(true);  // enable to object.
        enemyToSpawn.transform.position = position;
        enemyToSpawn.transform.rotation = rotation;

        enemyPoolDictionary[tag].Enqueue(enemyToSpawn);  

        return enemyToSpawn;
    }

}
