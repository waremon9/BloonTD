using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectileMovement : MonoBehaviour
{
    protected float lifeTime;
    protected float speed;

    [Header("Projectile script")]
    [SerializeField] protected BaseProjectile projectile;

    public void Initialize(float speed, float lifeTime)
    {
        this.speed = speed;
        this.lifeTime = lifeTime;
    }

    private void Update()
    {
        Movement();
    }

    protected abstract void Movement();
}
