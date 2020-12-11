using UnityEngine;
using UnityEngine.AI;

public class MoveToCircle : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private GameObject _player;
    private GameObject _agent;
    private Vector3 _playerPreviousPos;
    #endregion

    public MoveToCircle(GameObject agent, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = agent;
    }

    #region Interface Functions

    public void OnEnter()
    {
        _animator.SetBool("Fox_Run", true);
        _playerPreviousPos = _player.transform.position;
        #region SetDestinationFromSquare
        _agent.GetComponent<FoxAgentFSM>().destination = (_agent.GetComponent<FoxAgentFSM>().squareNormalisedPosition * _agent.GetComponent<FoxAgentFSM>().circleRadius) + _player.transform.position;
        NavMeshHit hit;
        int sampleDistance = 1;
        while (!NavMesh.SamplePosition(_agent.GetComponent<FoxAgentFSM>().destination, out hit, sampleDistance, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            sampleDistance++;
        }
        _agent.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        _navMeshAgent.SetDestination(_agent.GetComponent<FoxAgentFSM>().destination);
        #endregion
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
        _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
    }

    public void TimeTick()
    {
        NavMeshHit hit;
        int sampleDistance = 1;
        while (!NavMesh.SamplePosition(_agent.GetComponent<FoxAgentFSM>().destination + (_player.transform.position - _playerPreviousPos), out hit, sampleDistance, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            sampleDistance++;
        }
        _agent.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        Debug.Log("SamplePosition On Circle Adjusted for Player movement=" + hit.position);
        _navMeshAgent.SetDestination(_agent.GetComponent<FoxAgentFSM>().destination);
        _playerPreviousPos = _player.transform.position;
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
    }
    #endregion
}
