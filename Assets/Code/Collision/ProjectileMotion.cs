using System;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public bool projectileIsAlive = false;
    public Vector3 velocity;
    private Vector3 objPosition;
    public GameObject projectilePrefab;
    public Transform objTransform;
    public float initialSpeed;
    public float elevationAngle;
    public const float g = -9.82f;
    private float timeToLive = 4;

    public void CreateProjectile(Vector3 position, Quaternion rotation, Transform player)
    {
        GameObject temp = GameObject.Instantiate(projectilePrefab, position, rotation) as GameObject;
        temp.transform.localScale = new Vector3(9, 9, 9); //SCALING on prefab is fucked beyond saving thus magic numbers!

        //calculate size of horizontal movement and multiply the value in X and Z by their proportions of the unit vector whit Z replacing Y.
        var horizontalSpeed = (float)Math.Cos(temp.GetComponent<ProjectileMotion>().elevationAngle) * temp.GetComponent<ProjectileMotion>().initialSpeed;
        //Vector3 localForward = objTransform.worldToLocalMatrix.MultiplyVector(objTransform.forward);
        //localForward.y = 0;
        //localForward.Normalize();
        temp.GetComponent<ProjectileMotion>().velocity += new Vector3(Math.Abs(horizontalSpeed), (float)Math.Sin(temp.GetComponent<ProjectileMotion>().elevationAngle) * temp.GetComponent<ProjectileMotion>().initialSpeed);


        temp.transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
        temp.GetComponent<ProjectileMotion>().projectileIsAlive = true;
        temp.GetComponent<WeaponCollision>().collisionActive = true;
    }

    public void Update()
    {
        if (projectileIsAlive)
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive < 0)
            {
                Destroy(this.transform.root.gameObject);
            }
            objPosition = objTransform.position;
            velocity.y += Vy_iterative(Time.deltaTime, g);
            objPosition.y += Ypos_iterative(Time.deltaTime, velocity.y, g);
            objPosition += Horizontal_Iterative();
            objTransform.position = objPosition;
        }
    }

    private Vector3 Horizontal_Iterative()
    {
        var temp = transform.forward;
        temp.y = 0;
        temp.Normalize();
        return (temp * velocity.x * Time.deltaTime);
    }


    private float Vy_iterative(float dt, float acceleration = g)
    {
        return dt * acceleration;
    }

    private float Xpos_iterative(float dt, float Vx)
    {
        float X = dt * Vx;
        return X;
    }

    private float Ypos_iterative(float dt, float Vy, float acceleration = g)
    {
        float Y = dt * Vy - (float)Math.Pow(dt, 2) * acceleration / 2;
        return Y;
    }

    private float Zpos_iterative(float dt, float Vz)
    {
        float Z = dt * Vz;
        return Z;
    }
}
