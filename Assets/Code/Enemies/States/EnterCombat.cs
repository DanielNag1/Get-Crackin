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
         /*
         CM.AssignCombatRole(this);
         CM.SetCircleRange(this);
         */
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
