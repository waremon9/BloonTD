using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Move script")]
    [SerializeField] protected BaseProjectileMovement MoveComponent;

    protected int damage;

    public ProjectileType projectileType;

    [HideInInspector] public float hitBoxRadius;

    private List<BaseBalloon> allBalloonsHit = new List<BaseBalloon>();

    private void Start()
    {
        ProjectileManager.Instance.AddProjectile(this);
        
        if (TryGetComponent(out CircleCollider2D collider))
        {
            hitBoxRadius = collider.radius * transform.localScale.x;
            collider.enabled = false;
        }
    }
    
    public void InitializeBaseStats(int damage, float speed, float lifeTime)
    {
        this.damage = damage;
        MoveComponent.Initialize(speed, lifeTime);
    }
    
    public virtual void EndOfLife()
    {
        ProjectileManager.Instance.RemoveProjectile(this);
        Destroy(gameObject);
    }

    public abstract void BalloonHit(BaseBalloon balloon);
}

public enum ProjectileType{Standard, Fire, Ice};