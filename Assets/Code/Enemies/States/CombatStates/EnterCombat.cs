using UnityEngine;

public class EnterCombat : IState
{
    #region Variables
    public bool finishedEnteringCombat = false;
    public bool ableToEnterCombat = false;
    private GameObject _gameObject;
    private float _talkingDistance;
    #endregion

    public EnterCombat(GameObject enemy, float talkingDistance)
    {
        this._gameObject = enemy;
        this._talkingDistance = talkingDistance;
    }

    #region Interface Functions
    public void OnEnter()
    {
        Debug.Log("skit" + _gameObject.GetInstanceID());
        EnemyManager.Instance.AgentDetectedPlayer(_gameObject, _talkingDistance);
        EnemyManager.Instance.AssignCombatRoleAndCircleRadius();
        if (EnemyManager.Instance.AssignSquare(_gameObject))
        {
            ableToEnterCombat = true;
            finishedEnteringCombat = true;
        }
        else
        {
            ableToEnterCombat = false;
        }
    }

    public void OnExit()
    {
        finishedEnteringCombat = false;
        ableToEnterCombat = false;
    }

    public void TimeTick()
    {

    }
    #endregion
}
