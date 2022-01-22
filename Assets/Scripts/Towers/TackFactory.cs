using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class TackFactory : ProjectileTower
{
    [Header("Projectile")]
    public TackPile projectile;
    public int tackQte;

    private List<Vector3> allDestination = new List<Vector3>();

    public override void Shoot()
    {
        tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
            ProjectileManager.Instance.projectileParent);
        
        tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
        ((TackPile)tempProjCreated).SetTackQte(tackQte);

        Vector3 destination;
        if (allDestination.Count == 0)
            destination = (Vector3) Random.insideUnitCircle * (range / 2) + transform.position;
        else destination = allDestination[Random.Range(0, allDestination.Count - 1)];
        
        ((TackPile)tempProjCreated).GetComponent<GoToAndStayInPlace>().SetDestination(destination);
    }

    protected override bool IsTargetInRange()
    {
        return !EnemiesManager.Instance.waveEnded;
    }

    private void GetPileDestination()
    {
        Spline path = EnemiesManager.Instance.path;
        float point = 0.2f;

        for (int i = 1; i < path.Length / point; i++)
        {
            Vector3 temp = path.GetSampleAtDistance(point * i).location+path.transform.position;
            if (Vector3.Distance(temp, transform.position) <= range)
            {
                allDestination.Add(temp);
            }
        }
    }

    protected override void OnTowerEnable()
    {
        GetPileDestination();
    }
}
