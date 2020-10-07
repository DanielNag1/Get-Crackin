using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private Health health;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name== "HealthHitBox")
        {
            //animator.SetTrigger("GetHit");
            //health = collision.gameObject.GetComponentInChildren<Health>();
            //health.ModifyHealth(-5);
        }
    }

}