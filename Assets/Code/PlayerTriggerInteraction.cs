using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInteraction : MonoBehaviour
{
    private TriggerComponent callableUnit;
    private LevelManager levelManager;
    private EnemyManager enemyManager;
    private EnemySpawner enemySpawner;

    private void Awake()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Trigger")
        {
            Debug.Log("Collided with Trigger");
            callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
            callableUnit.ActivateTrigger();
            Debug.Log("Done");
        }
        if (hit.collider.gameObject.tag == "LevelTrigger")
        {
            LoadNextLevel(hit);
        }
        if (hit.collider.gameObject.tag == "EnemyWave1")
        {
            SpawnEnemies(hit);
        }
    }
    #region Method
    private void LoadNextLevel(ControllerColliderHit hit)
    {
        callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
        callableUnit.ActivateTrigger();
        levelManager.LoadNextLevel();
    }
    private void SpawnEnemies(ControllerColliderHit hit)
    {
        callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
        enemySpawner = GetComponent<EnemySpawner>();
        callableUnit.ActivateTrigger();
        enemyManager.SpawnEnemyFromTrigger();
    }
    #endregion
}
