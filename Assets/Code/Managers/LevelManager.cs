using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    #region Variables

    public NavMeshSurface navMeshSurface;

    #endregion

    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }

}
