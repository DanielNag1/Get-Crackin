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
    /*
     Needed Agent states:
     EnterCombat(entery point, Gets assigned a circle by the CM)
    X MoveToCircle
    X EncircleTarget (Waits for CM to give attack command)
     [Melee]
    X ApproachTarget (approches until target is within attack range) tranisitons to AttackTarget and StepBackToCircle[if position is further from target then circle radius]
    X AttackTarget (Does one attack to the target)
    X StepBackToCircle (Informs CM that it has finished attacking)  == encircle target
     [Range]
    X ApproachTarget (approches until target is within attack range) tranisitons to AttackTarget and StepBackToCircle[if position is further from target then circle radius]
    X AttackTarget (Does one attack to the target)
    D GetAmmo (Checks if an ammo stockpile is within reconable range, if so goes to target, Informs CM that it has finished attacking)
    X MoveToPosition (ammo stockpile)
    X Reload
    X StepBackToCircle (Informs CM that it has finished attacking)  == encircle target
    */



    //Walks into a safe distance cirkle from the player
    //CM gives permission to one or two enemies to attack
    //The enemies approach to attacking disatance
    //The enemies attack
    //They say they finished attacking allowing CM to give other ai permission
    //the enemies walk back to the safe cirkle




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
