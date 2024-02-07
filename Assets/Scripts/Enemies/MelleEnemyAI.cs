using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MelleEnemyAI : MonoBehaviour
{
    [SerializeField] private float attackCooldawn;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D area;
    [SerializeField] private float range;

    private float currentAttackCooldawn = Mathf.Infinity;

    private Transform target;
    [SerializeField] private float speed;

    
    
    Rigidbody2D rb;



    private float prevFramePosX = 0;
    private float currentFramePosX = 1;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }


    private void Update()
    {
        currentFramePosX = transform.position.x;
        if(currentFramePosX - prevFramePosX > 0.005f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (currentFramePosX - prevFramePosX < -0.005f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        if (PlayerInSight()) {
            this.GetComponent<AIPath>().enabled = false;
            if (currentAttackCooldawn < 0)
            {
                //Attack
                currentAttackCooldawn = attackCooldawn;
            }
        }
        else
        {
            this.GetComponent<AIPath>().enabled = true;
        }
        print(rb.velocity);

        prevFramePosX = transform.position.x;
    }

    private bool PlayerInSight()
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
