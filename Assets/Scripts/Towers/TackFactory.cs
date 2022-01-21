using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class TackFactory : ProjectileTower
{
    [Header("Projectile")]
    public TackPile projectile;
    public int tackQte;
    
    public override void Shoot()
    {
        tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
            ProjectileManager.Instance.projectileParent);
        
        tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
        ((TackPile)tempProjCreated).SetTackQte(tackQte);
        
        Spline spline = EnemiesManager.Instance.path;
        Vector3 pointToProject = transform.position + (Vector3)Random.insideUnitCircle * range;
        Vector3 destination = spline.GetProjectionSample(pointToProject - spline.transform.position).location + spline.transform.position;//don't ask pls

        if (Vector3.Distance(destination, transform.position) > range)
        {
            destination = pointToProject;
        }
        
        ((TackPile)tempProjCreated).GetComponent<GoToAndStayInPlace>().SetDestination(destination);
    }

    protected override bool IsTargetInRange()
    {
        return !EnemiesManager.Instance.IsWaveFinished();
    }
}
