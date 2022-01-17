using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Shop")]
    [SerializeField] private int cost;

    [Header("Bullet basic property")]
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileLifetime;
    [SerializeField] protected int damage;

    [Header("Tower range")]
    [SerializeField] protected float range;
    [SerializeField] protected GameObject rangeIndicator;
    
    [Header("Tower attack speed")]
    [SerializeField] private float reloadTime;
    protected float LastShoot = -9999f;

    protected BaseProjectile tempProjCreated;

    protected BaseBalloon target;
    
    private void Start()
    {
        UpdateRangeIndicator();
    }

    protected virtual void UpdateRangeIndicator()
    {
        rangeIndicator.transform.localScale = Vector2.one * range * 2;
    }

    protected virtual void Update()
    {
        if (CanShoot() && EnemiesManager.Instance.AtLeastOneBalloonInRange(transform.position, range))
        {
            UpdateTarget();
            RotationLookAtTarget();
            Shoot();
            LastShoot = Time.time;
        }
    }
    
    protected void RotationLookAtTarget()
    {
        transform.up = target.transform.position - transform.position;
    }

    protected bool CanShoot()
    {
        return Time.time >= LastShoot + reloadTime ;
    }
    
    public abstract void Shoot();
    
    protected virtual void UpdateTarget()
    {
        target = EnemiesManager.Instance.GetFirstBalloonInRange(transform.position, range);
    }

    public void EnableRangeIndicator(bool b)
    {
        rangeIndicator.SetActive(b);
    }
    
    private void OnValidate()
    {
        UpdateRangeIndicator();
    }
}
