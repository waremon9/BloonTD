using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : BaseTower
{
    [Header("Bullet used")]
    [SerializeField] private PierceProjectile projectile;

    [Header("Special property")]
    [SerializeField] private int piercePower;
    
    public override void Shoot()
    {
        tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
            ProjectileManager.Instance.projectileParent);
        
        tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
        ((PierceProjectile)tempProjCreated).SetPierce(piercePower);
    }
}
