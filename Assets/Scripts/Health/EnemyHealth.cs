using UnityEngine;
using System.Collections;
using UnityEditor;
using Pathfinding;

public class EnemyHealth : Health
{
    protected void Update()
    {
    }
    override protected void Death()
    {
        base.Death();
        anim.SetTrigger("die");
        GetComponent<MelleEnemyAI>().enabled = false;
        GetComponent<AIPath>().enabled = false;
        dead = true;
        //возможно стоит Deactivate()
        Destroy(gameObject, 5);
    }
}