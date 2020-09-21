using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtful : Weapon
{
    PlayerHealth player = default;
    void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.GetComponent<PlayerHealth>();
        Attack();
    }

    protected override void Attack()
    {
        player.CurrentHealth -= damage;
    }
}
