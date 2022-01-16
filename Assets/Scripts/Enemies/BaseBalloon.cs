using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseBalloon : MonoBehaviour
{
    public float speed = 1;
    [SerializeField] private BaseBalloon[] releaseOnDeath;

    [SerializeField] private FollowSpline followSpline;
    public FollowSpline FollowSpline
    {
        get { return followSpline; }
    }

    [SerializeField] private List<ProjectileType> resistance = new List<ProjectileType>();

    [HideInInspector] public float hitBoxRadius;
    
    public void Initialize(Spline s, float speed, float dist = 0)
    {
        followSpline.spline = s;
        followSpline.dist = dist;
        
        followSpline.UpdatePosition();
    }

    public void AddResistance(ProjectileType type)
    {
        resistance.Add(type);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Hit();
        }
    }

    private void Start()
    {
        if (!followSpline)
        {
            if(!TryGetComponent(out followSpline)) Debug.Log("Balloon has no 'FollowSpline' component. " + name);
        }

        if (TryGetComponent(out CircleCollider2D collider))
        {
            hitBoxRadius = collider.radius * transform.localScale.x;
            collider.enabled = false;
        }
    }

    protected void Death(int damage = 0)
    {
        EnemiesManager.Instance.BalloonDead(this);

        if (EnemiesManager.Instance.BalloonPopEffect && GameManager.Instance.particleParent)
        {
            ParticleSystem temp = Instantiate(EnemiesManager.Instance.BalloonPopEffect, transform.position,
                        Quaternion.identity, GameManager.Instance.particleParent);
        }
        if (!EnemiesManager.Instance.BalloonPopEffect)
        {
            Debug.LogError("Missing particle system prefab : " + name);
        }
        if (!GameManager.Instance.particleParent)
        {
            Debug.LogError("No particle parent reference in gameManager");
        }

        int i = 0;
        foreach (BaseBalloon baseBalloon in releaseOnDeath)
        {
            if(baseBalloon) EnemiesManager.Instance.EnemieSpawn(baseBalloon, followSpline.dist - i * 0.3f, damage);

            i++;
        }
        
        Destroy(gameObject);
    }

    virtual public void Hit(int damage = 1)
    {
        if(damage == 0) return;
        Death(damage - 1);
    }
    
    public bool IsResistant(ProjectileType type)
    {
        foreach (ProjectileType projectileType in resistance)
        {
            if (type == projectileType)
            {
                return true;
            }
        }

        return false;
    }

    public float GetWorlHitBoxRadius()
    {
        return hitBoxRadius * transform.localScale.x;   
    }
}
