using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float currentHealth = default;
    [SerializeField] protected float maxHealth = default;
    
    [SerializeField] protected bool alive = default;
    [SerializeField] AudioSource hitSound = default;

    protected float currentInvulnerabilityTime = default;
    [SerializeField] protected float invulnerabilityTime = default;
    [SerializeField] protected bool invulnerable = false;

    [SerializeField] float knockbackForce = default;

    public float CurrentHealth
    {
        get 
        {
            return currentHealth;
        }
        set
        {
            if (invulnerable) return;

            if(value - currentHealth < 0)
            {
                HitSound();
                invulnerable = true;
                currentInvulnerabilityTime = 0;
            }

            currentHealth = value;
            if(currentHealth <= 0)
            {
                Death();
            }
        }
    }
    private void Awake()
    {
        currentHealth = maxHealth;
        alive = true;
    }

    public void ApplyKnockback(Transform knockbacker)
    {
        Vector2 knockbackDirection = new Vector2(this.transform.position.x - knockbacker.position.x, this.transform.position.y - knockbacker.position.y);
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if(invulnerable)
        {
            currentInvulnerabilityTime += Time.deltaTime;
            if(currentInvulnerabilityTime >= invulnerabilityTime)
            {
                invulnerable = false;
            }
        }
    }

    void HitSound()
    {
        if (!hitSound.isPlaying)
            hitSound.Play();
    }
    public virtual void Death()
    {
        alive = false;
        Destroy(this.gameObject);
    }
}
