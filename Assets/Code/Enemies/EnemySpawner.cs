using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    EnemyManager EnemyManager;
    [SerializeField]
    List<GameObject> spawnPoints = new List<GameObject>();

   
    private void Start()
    {
        EnemyManager = EnemyManager.Instance;

    }
    private void FixedUpdate()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EnemyManager.SpawnEnemyFromPool("Enemy", spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
  
}

