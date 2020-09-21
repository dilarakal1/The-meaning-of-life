using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] Transform spawnPoint = null;

    public override void Death()
    {
        currentHealth = maxHealth;
        alive = true;
        this.transform.position = spawnPoint.position;
    }
}
