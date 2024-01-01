using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleEnemyAI : MonoBehaviour
{
    [SerializeField] private float attackCooldawn;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;
    private float cooldawnTimer = Mathf.Infinity;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private float range;

    private void Update()
    {
        cooldawnTimer += Time.deltaTime;

        if (PlayerInSight()) {
            if (cooldawnTimer > attackCooldawn)
            {
                //Attack
            }
        }   
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x), 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private bool WithinArea()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


}
