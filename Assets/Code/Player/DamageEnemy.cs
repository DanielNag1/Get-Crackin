using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] RageMode rage;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //rage.ModifyRage(25);
            //collision.gameObject.GetComponent<enemyhealth>().MakeDamage(15);
        }
    }
}
