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
    
    [Header("Tower attack speed")]
    [SerializeField] private float reloadTime;
    protected float LastShoot = -9999f;
    
    [Header("Rotation")]
    [SerializeField] protected bool rotateToLook = true;

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
        if (!rotateToLook) return;
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
            rangeIndicator.GetComponent<SpriteRenderer>().color = GameManager.Instance.collisionColor.var;
        }
        else
        {
            rangeIndicator.GetComponent<SpriteRenderer>().color = GameManager.Instance.noCollisionColor.var;
        }
    }
    
    private void OnValidate()
    {
        UpdateRangeIndicator();
    }
}
