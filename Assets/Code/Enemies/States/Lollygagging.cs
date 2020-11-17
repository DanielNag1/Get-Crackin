using UnityEngine;
using UnityEngine.AI;

public class Lollygagging : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private Rigidbody rb;
    public Vector3 targetPos;
    #endregion

    public Lollygagging(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        rb = enemy.GetComponent<Rigidbody>();

    }

    #region Interface Functions

    public void OnEnter()
    {
        rb.isKinematic = false;
        float angle = Random.Range(0, 360);
        Vector3 HomeVector = new Vector3(_navMeshAgent.transform.root.position.x, _navMeshAgent.transform.root.position.y, _navMeshAgent.transform.root.position.z);
        Vector3 randomDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));//get random direction        
        randomDirection.Normalize(); //normalize
        randomDirection *= _navMeshAgent.speed * (Random.Range(15, 35) / 10);//randomize how much time it should take to walk
        randomDirection += HomeVector;  //transplant the position to our spawn position (Want to change so it's based on agent position.
        NavMeshHit hit;
        int i = 1;
        while (!NavMesh.SamplePosition(randomDirection, out hit, i, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            i++;
        }
        targetPos = hit.position; //set as target position.
        if (NavMesh.Raycast(hit.position, HomeVector, out hit, NavMesh.AllAreas))
        {
            targetPos = hit.position; //set as target position.
        }
        targetPos.y = targetPos.y + 1;
        _navMeshAgent.enabled = true;
        _animator.SetBool("Fox_Walk", true);
    }
    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Fox_Walk", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
        _navMeshAgent.SetDestination(targetPos);
    }
    #endregion
}
