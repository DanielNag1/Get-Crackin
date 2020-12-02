using UnityEngine;
using UnityEngine.AI;

public class EnterCombat : IState
{
    #region Variables
    public bool finishedEnteringCombat = false;
    private GameObject gameObject;
    private float talkingDistance;
    #endregion

    public EnterCombat(GameObject enemy, float talkingDistance)
    {
        gameObject = enemy;
        this.talkingDistance = talkingDistance;
    }

    #region Interface Functions

    public void OnEnter()
    {
        EnemyManager.Instance.AgentDetectedPlayer(gameObject, talkingDistance);
        EnemyManager.Instance.AssignCombatRoleAndCircleRadius(gameObject);
        EnemyManager.Instance.AssignSquare(gameObject);
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
