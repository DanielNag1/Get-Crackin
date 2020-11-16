using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject item;
    public float radius = 1;
    public Vector3 regionSize = Vector3.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    List<Vector3> points;

    

    void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        if (points != null)
        {
            foreach (Vector3 point in points)
            {
                GameObject clone = Instantiate(item, point, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionSize / 2, regionSize);


    }
}
