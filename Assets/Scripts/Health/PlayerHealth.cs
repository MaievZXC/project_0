using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerHealth : Health
{

    override protected IEnumerator Death()
    {
        anim.SetTrigger("die");
        GetComponent<PlayerMovement>().enabled = false;
        dead = true;
        yield return new WaitForSeconds(0);
    }
}