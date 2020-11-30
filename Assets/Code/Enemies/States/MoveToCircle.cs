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
        Debug.Log("START");
        Debug.Log("_playerPreviousPos =" + _playerPreviousPos);
        _playerPreviousPos = _player.transform.position;
        Debug.Log("_agent current position=" + _agent.transform.position);
        Debug.Log(" _agent previous destination=" + _agent.GetComponent<FoxAgentFSM>().destination);

        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);

        _navMeshAgent.destination = _player.transform.position;

        _agent.GetComponent<FoxAgentFSM>().destination = _navMeshAgent.destination;
        Debug.Log("_agent current destination=" + _agent.GetComponent<FoxAgentFSM>().destination);

        Vector3 fromAgentToPlayer = _navMeshAgent.transform.position - _player.transform.position;
        Debug.Log("fromAgentToPlayer=" + fromAgentToPlayer);
        fromAgentToPlayer.Normalize();
        Debug.Log("Normalized=" + fromAgentToPlayer);
        fromAgentToPlayer *= _agent.GetComponent<FoxAgentFSM>().circleRadius;
        Debug.Log("Multiplied=" + fromAgentToPlayer);
        _agent.GetComponent<FoxAgentFSM>().destination = _agent.GetComponent<FoxAgentFSM>().destination + fromAgentToPlayer;
        Debug.Log("Destination On Circle=" + _agent.GetComponent<FoxAgentFSM>().destination);
        NavMeshHit hit;
        int i = 1;
        while (!NavMesh.SamplePosition(_agent.GetComponent<FoxAgentFSM>().destination, out hit, i, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            i++;
        }
        _agent.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        Debug.Log("SamplePosition On Circle=" + _agent.GetComponent<FoxAgentFSM>().destination);
        _navMeshAgent.SetDestination(_agent.GetComponent<FoxAgentFSM>().destination);
        Debug.Log("STOP");
        EnemyManager.Instance.Evade(_agent);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
        _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
    }

    public void TimeTick()
    {
        _animator.SetBool("Fox_Run", true);
        Debug.Log("STARTTick");
        NavMeshHit hit;
        int i = 1;
        while (!NavMesh.SamplePosition(_agent.GetComponent<FoxAgentFSM>().destination + (_player.transform.position - _playerPreviousPos), out hit, i, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            i++;
        }
        _agent.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        Debug.Log("SamplePosition On Circle=" + _agent.GetComponent<FoxAgentFSM>().destination);
        _navMeshAgent.SetDestination(_agent.GetComponent<FoxAgentFSM>().destination);
        _playerPreviousPos = _player.transform.position;
        Debug.Log("STOPTick");
    }
    #endregion
}
