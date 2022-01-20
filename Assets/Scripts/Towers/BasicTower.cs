using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : ProjectileTower
{
    [Header("Bullet used")]
    [SerializeField] protected PierceProjectile projectile;

    [Header("Special property")]
    [SerializeField] protected int piercePower;
    
    public override void Shoot()
    {
        tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
            ProjectileManager.Instance.projectileParent);
        
        tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
        ((PierceProjectile)tempProjCreated).SetPierce(piercePower);
    }
}
