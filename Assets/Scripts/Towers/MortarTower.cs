using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MortarTower : NoProjectileTower
{
    [Header("Special property")]
    [SerializeField] private Transform whereToShoot;
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float explosionRange;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private float spread;

    private bool moveTarget = false;
    private Vector3 whereToShootBeforeUpdate;
    
    protected override void Update()
    {
        if (!enabledTower) return;
        
        if (CanShoot() && IsTargetInRange())
        {
            Shoot();
            LastShoot = Time.time;
        }

        if (moveTarget)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            whereToShoot.transform.position = new Vector3(position.x, position.y, 0);
            if (Input.GetMouseButtonDown(1))
            {
                moveTarget = false;
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    whereToShootBeforeUpdate = whereToShoot.position;
                    towerTransform.up = whereToShootBeforeUpdate - transform.position;
                }
                else
                {
                    whereToShoot.position = whereToShootBeforeUpdate;
                }

                
            }
        }
    }

    protected override bool IsTargetInRange()
    {
        return GameManager.Instance.gameState == GameState.WaveComing;
    }

    private Vector3 p;
    
    public override void Shoot()
    {
        Vector3 impactPoint = whereToShootBeforeUpdate + (Vector3)Random.insideUnitCircle * spread;
        p = impactPoint;
        foreach (BaseBalloon b in EnemiesManager.Instance.enemiesParent.GetComponentsInChildren<BaseBalloon>())
        {
            if (Vector3.Distance(b.transform.position, impactPoint) <= explosionRange + b.hitBoxRadius)
            {
                if(!b.IsResistant(type)) b.Hit(damage); 
            }
        }

        if (explosionEffect && GameManager.Instance.particleParent)
        {
            ParticleSystem temp = Instantiate(explosionEffect, impactPoint, Quaternion.identity, GameManager.Instance.particleParent);
            temp.transform.localScale = Vector3.one * explosionRange;
        }
        if (!explosionEffect)
        {
            Debug.LogError("Missing particle system prefab : " + name);
        }
        if (!GameManager.Instance.particleParent)
        {
            Debug.LogError("No particle parent reference in gameManager");
        }
    }

    protected override void OnTowerEnable()
    {
        whereToShootBeforeUpdate = whereToShoot.position;
        whereToShoot.gameObject.SetActive(true);
        StartCoroutine(MoveTargetCoroutine());
    }

    public override void TowerGetSelected()
    {
        base.TowerGetSelected();
        StartCoroutine(MoveTargetCoroutine());
        whereToShoot.gameObject.SetActive(true);
    }

    public override void TowerGetUnselected()
    {
        base.TowerGetUnselected();
        whereToShoot.gameObject.SetActive(false);
    }

    private IEnumerator MoveTargetCoroutine()
    {
        yield return null;
        moveTarget = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(p, explosionRange);
    }
}
