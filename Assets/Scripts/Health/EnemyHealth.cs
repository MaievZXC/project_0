using UnityEngine;
using System.Collections;
using UnityEditor;
using Pathfinding;

public class EnemyHealth : Health
{
    override protected void Death()
    {
        base.Death();
        //возможно стоит Deactivate()
        Destroy(gameObject, 5);
    }
}