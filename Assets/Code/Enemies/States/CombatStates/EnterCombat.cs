using UnityEngine;
using UnityEngine.AI;

public class EnterCombat : IState
{
    #region Variables
    public bool finishedEnteringCombat = false;
    #endregion

    public EnterCombat(GameObject enemy)
    {

    }

    #region Interface Functions

    public void OnEnter()
    {
        /*
         CM.AddAgentToCombatList(this);
         CM.AlertAgentsInVecinity(this);
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
