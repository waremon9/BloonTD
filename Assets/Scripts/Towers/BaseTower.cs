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

    private List<BaseBalloon> BalloonsInRange = new List<BaseBalloon>();
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
        UpdateBalloonInRange();
        
        if (CanShoot() && TargetInRange())
        {
            CleanBalloonInRange();

            if (TargetInRange())
            {
                UpdateTarget();
                RotationLookAtTarget();
                Shoot();
                LastShoot = Time.time;
            };
        }
    }

    protected virtual void UpdateBalloonInRange()
    {
        BalloonsInRange.Clear();

        foreach (BaseBalloon balloon in EnemiesManager.Instance.GetAllBalloon())
        {
            if (Vector3.Distance(transform.position, balloon.transform.position) <= range)
            {
                BalloonsInRange.Add(balloon);
            }
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

    protected virtual bool TargetInRange()
    {
        return BalloonsInRange.Count != 0;
    }

    public abstract void Shoot();
    
    protected virtual void UpdateTarget()
    {
        BaseBalloon t = BalloonsInRange[0];

        foreach (BaseBalloon balloon in BalloonsInRange)
        {
            if (balloon.FollowSpline.rate > t.FollowSpline.rate)
            {
                t = balloon;
            }
        }
        
        target = t;
    }
    
    //remove balloon dead while in range
    private void CleanBalloonInRange()
    {
        for (int i = BalloonsInRange.Count - 1; i >= 0; i--)
        {
            if (BalloonsInRange[i]) 
            {
                continue;
            }
            BalloonsInRange.RemoveAt(i);
        }
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
