using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region Singleton
    public static CombatManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    private Dictionary<int, GameObject> _enemies;
    private Semaphore _allowedToAttack = new Semaphore(0, 2);
    private float _meleeCircleRadius = 3f;
    private float _rangeCircleRadius = 9f;
    private float _separationFactor = 1f;
    #endregion

    #region Methodes
    public CombatManager()
    {

    }


    #region DictionaryManagement
    /// <summary>
    /// Adds an enemy to the combat manager when they enter the combat state block
    /// </summary>
    public void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy.GetInstanceID(), enemy);
    }
    
    /// <summary>
    /// Removes an enemy from the combat manager when they leave the combat state block
    /// </summary>
    public void RemoveEnemy(GameObject enemy)
    {
        if (!_enemies.Remove(enemy.GetInstanceID()))//returns true if remove succeded.
        {
            Debug.Log("Enemy: " + enemy + " nor removed!");
        }
    }
    #endregion

    #endregion
}
