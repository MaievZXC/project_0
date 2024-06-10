using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] Waves;
    [SerializeField] private AudioClip projectileSound;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //add various input devices?
        if (Input.GetMouseButton(1) && cooldownTimer > attackCooldown && playerMovement.canAttack() && !anim.GetBool("Hurt"))
        {
            StartCoroutine(Attack2());
        }
        else if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack() && !anim.GetBool("Hurt"))
        {
            StartCoroutine(Attack());
        }

        cooldownTimer += Time.deltaTime;
    }

    private IEnumerator Attack2()
    {
        SoundManager.instance.PlaySound(projectileSound);
        anim.SetTrigger("attack_2");
        cooldownTimer = 0;
        playerMovement.setVelosity0();
        yield return new WaitForSeconds(attackCooldown);
        Waves[FindWave()].transform.position = firePoint.position;
        Waves[FindWave()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private IEnumerator Attack()
    {
        Collider2D[] objectsHit = Physics2D.OverlapBoxAll(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x),
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, enemyLayer);
        //другой звук надо поставить
        SoundManager.instance.PlaySound(projectileSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        playerMovement.setVelosity0();
        yield return new WaitForSeconds(attackCooldown);
        foreach (var target in objectsHit){
            target.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);
        }
    }
    private int FindWave()
    {
        for (int i = 0; i < Waves.Length; i++)
        {
            if (!Waves[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public float GetCooldawn()
    {
        return attackCooldown;
    }
    public float GetTimer()
    {
        return cooldownTimer;
    }
}