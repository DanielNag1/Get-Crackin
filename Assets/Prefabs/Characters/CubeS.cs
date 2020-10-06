using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubeS : MonoBehaviour
{
    public Transform ss;
    private NavMeshAgent nm;

    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nm.transform.LookAt(ss.position);
        nm.SetDestination(ss.position);
    }
}
