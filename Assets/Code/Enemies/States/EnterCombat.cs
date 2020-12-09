using UnityEngine;

public class EnterCombat : IState
{
    #region Variables
    public bool finishedEnteringCombat = false;
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
        EnemyManager.Instance.AgentDetectedPlayer(_gameObject, _talkingDistance);
        EnemyManager.Instance.AssignCombatRoleAndCircleRadius(_gameObject);
        EnemyManager.Instance.AssignSquare(_gameObject);
        finishedEnteringCombat = true;
    }

    public void OnExit()
    {
        finishedEnteringCombat = false;
    }

    public void TimeTick()
    {

    }
    #endregion
}
