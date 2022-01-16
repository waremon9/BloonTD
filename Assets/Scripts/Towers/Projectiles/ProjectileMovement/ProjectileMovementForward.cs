using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovementForward : BaseProjectileMovement
{
    protected override void Movement()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            projectile.EndOfLife();
        }
    }
}
