using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth = default;
    [SerializeField] float maxHealth = default;
    [SerializeField] bool alive = default;
    [SerializeField] AudioSource hitSound = default;

    public float CurrentHealth
    {
        get 
        {
            return currentHealth;
        }
        set
        {
            if(value - currentHealth < 0)
            {
                HitSound();
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
