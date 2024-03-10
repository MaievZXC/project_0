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

    private float currentAttackCooldawn = -1000;
    [SerializeField] private float speed;

    
    

    Animator anim;



    private float prevFramePosX = 0;
    private float currentFramePosX = 1;
    RaycastHit2D hit;


    private void Start()
    {
        anim = GetComponent<Animator>();

    }


    private void Update()
    {
        СontactDamage();

        hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        currentFramePosX = transform.position.x;

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
        //Если не в области, то скрипт построения пути не работает, также он не работает во время анимации атаки и когда атакован
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
        //Если захочу сделать чтобы атака сбивалась надо будет добавить проверку на Hurt
        yield return new WaitForSeconds(attackCooldawn);
        if(hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

    }

    private void СontactDamage()
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
