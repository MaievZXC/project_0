using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerHealth : Health
{

    override protected void Death()
    {
        base.Death();
        anim.SetTrigger("die");
        GetComponent<PlayerMovement>().enabled = false;
        dead = true;
    }
}