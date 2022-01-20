using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTower : BasicTower
{
    [SerializeField] private int bulletQte;

    public override void Shoot()
    {
        for (int i = 0; i < bulletQte; i++)
        {
            tempProjCreated = Instantiate(projectile, transform.position, transform.rotation, ProjectileManager.Instance.projectileParent);

            tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
            ((PierceProjectile)tempProjCreated).SetPierce(piercePower);

            float spread = (360 / bulletQte) * i;
            Vector3 rot = tempProjCreated.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x,rot.y ,rot.z + spread);
            tempProjCreated.transform.rotation = Quaternion.Euler(rot);
        }
    }
}
