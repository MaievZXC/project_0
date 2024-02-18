using UnityEngine;
using System.Collections;
using UnityEditor;
using Pathfinding;

public class EnemyHealth : Health
{

    override protected IEnumerator Death()
    {
        anim.SetTrigger("die");
        GetComponent<MelleEnemyAI>().enabled = false;
        GetComponent<AIPath>().enabled = false;
        dead = true;
        yield return new WaitForSeconds(5);
        Destroy(this);
    }
}