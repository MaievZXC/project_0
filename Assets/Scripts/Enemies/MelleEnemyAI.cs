using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngineInternal;

public class MelleEnemyAI : MonoBehaviour
{
    [SerializeField] private float attackCooldawn;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private GameObject area;
    [SerializeField] private GameObject target;

    private float currentAttackCooldawn = -1000;
    [SerializeField] private float speed;

    
    
    Rigidbody2D rb;
    Animator anim;



    private float prevFramePosX = 0;
    private float currentFramePosX = 1;
    RaycastHit2D hit;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }


    private void Update()
    {
        contactDamage();

        hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        currentFramePosX = transform.position.x;

        //его двигает не каждый кадр(
        print(currentFramePosX - prevFramePosX);
        anim.SetBool("Walk", true);
        if (currentFramePosX - prevFramePosX > 0.0005f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (currentFramePosX - prevFramePosX < -0.0005f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
        //≈сли не в области, то скрипт построени€ пути не работает, также он не работает во врем€ анимации атаки и когда атакован
        if (area.GetComponent<AgroWallDetector>().GetInArea() && currentAttackCooldawn < 0 && !anim.GetBool("Hurt")) {
            if (PlayerInAttackRange()) {
                GetComponent<AIPath>().enabled = false;
                if (currentAttackCooldawn < 0)
                {
                    anim.SetTrigger("Attack");
                    StartCoroutine(Attack());
                    currentAttackCooldawn = attackCooldawn;
                }
            }
            else
            {
                GetComponent<AIPath>().enabled = true;
            }
        }
        else
        {
            GetComponent<AIPath>().enabled = false;
        }

        prevFramePosX = transform.position.x;
        currentAttackCooldawn -= Time.deltaTime;
    }

    private bool PlayerInAttackRange()
    {
        hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range * 3f / 5, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }


    private bool PlayerInSight()
    {
        return hit.collider;
        
    }

    //private bool PlayerInArea() => area.

    private IEnumerator Attack()
    {
        //anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldawn);
        if(hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

    }

    private void contactDamage()
    {
        RaycastHit2D contactDamage = Physics2D.BoxCast(boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        if (contactDamage.collider != null && contactDamage.collider.gameObject.CompareTag("Player"))
        {
            contactDamage.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }




}
