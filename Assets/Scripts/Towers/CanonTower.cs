using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : ProjectileTower
{
    [Header("Bullet used")]
    [SerializeField] private Explosiveprojectiles projectile;

    [Header("Special property")]
    [SerializeField] private float explosionRange;
    
    public override void Shoot()
    {
        tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
            ProjectileManager.Instance.projectileParent);
        
        tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
        ((Explosiveprojectiles)tempProjCreated).SetExplosionRange(explosionRange);
    }
}
