using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileTower : BaseTower
{
    [Header("Projectile basic property")]
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileLifetime;
    [SerializeField] protected int damage;
    
    protected BaseProjectile tempProjCreated;
}
