using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseTower : MonoBehaviour
{
    [Header("collision")]
    public CircleCollider2D hitBox;
    
    [Header("Shop")]
    public int cost;

    [Header("Tower range")]
    [SerializeField] protected float range;
    [SerializeField] protected GameObject rangeIndicator;
    [SerializeField] private Color noCollisionColor;
    [SerializeField] private Color collisionColor;
    
    [Header("Tower attack speed")]
    [SerializeField] private float reloadTime;
    protected float LastShoot = -9999f;

    protected BaseBalloon target;
    
    [HideInInspector] public bool enabledTower = true;

    private void Start()
    {
        UpdateRangeIndicator();

        if (!hitBox)
        {
            if (!TryGetComponent(out hitBox))
            {
                Debug.LogError("No circle collider on tower " + name);
            }
        }
    }

    protected virtual void UpdateRangeIndicator()
    {
        rangeIndicator.transform.localScale = Vector2.one * range * 2;
    }

    protected virtual void Update()
    {
        if (!enabledTower) return;
        
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
        rangeIndicator.GetComponent<SpriteRenderer>().enabled = b;
    }

    public void SetCollisionColor(bool b)
    {
        if (b)
        {
            rangeIndicator.GetComponent<SpriteRenderer>().color = collisionColor;
        }
        else
        {
            rangeIndicator.GetComponent<SpriteRenderer>().color = noCollisionColor;
        }
    }
    
    private void OnValidate()
    {
        UpdateRangeIndicator();
    }
}
