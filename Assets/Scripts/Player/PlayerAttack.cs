using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] Waves;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && cooldownTimer > attackCooldown && playerMovement.canAttack() && anim.GetBool("Hurt") == false)
            StartCoroutine(Attack2());

        cooldownTimer += Time.deltaTime;
    }

    private IEnumerator Attack2()
    {
        anim.SetTrigger("attack_2");
        cooldownTimer = 0;
        playerMovement.setVelosity0();
        yield return new WaitForSeconds(attackCooldown);
        Waves[FindWave()].transform.position = firePoint.position;
        Waves[FindWave()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
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

    public float GetCooldawn2()
    {
        return attackCooldown;
    }
    public float GetTimer()
    {
        return cooldownTimer;
    }
}