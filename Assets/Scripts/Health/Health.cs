using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    public float currentHealth { get; private set; }
    protected Animator anim;
    protected bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private bool isInvulnerable;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Components to disable")]
    [SerializeField] private Behaviour[] components;

    protected void Awake()
    {
        dead = false;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void Respawn()
    {
        dead = false;
        foreach (Behaviour component in components)
            component.enabled = true;
        AddHealth(maxHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");

    }


    public void TakeDamage(float _damage)
    {
        if (!isInvulnerable)
        {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, maxHealth);

            if (currentHealth > 0)
            {
                SoundManager.instance.PlaySound(hitSound);
                anim.SetBool("Hurt", true);
                StartCoroutine(Invunerability());
            }
            else
            {
                if (!dead)
                {
                    Death();
                }
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, maxHealth);
    }
    private IEnumerator Invunerability()
    {
        isInvulnerable = true;
        for (int i = 0; i < numberOfFlashes / 2; i++)
        {
            spriteRend.color = new Color(1, 0.8160377f, 0.8160377f, 1);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        anim.SetBool("Hurt", false);
        for (int i = numberOfFlashes / 2; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0.8160377f, 0.8160377f, 1);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        isInvulnerable = false;
    }

    public bool IsMax()
    {
        return currentHealth == maxHealth;
    }

    virtual protected void Death()
    {
        foreach (Behaviour component in components)
            component.enabled = false;
        SoundManager.instance.PlaySound(deathSound);
        gameObject.layer = LayerMask.NameToLayer("Dead");
        dead = true;
        anim.SetTrigger("die");
    }
}
