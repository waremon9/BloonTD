using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : BasicTower
{
    [SerializeField] private float weaponSpread;
    [SerializeField] private int shellPerRound = 1;
    

    public override void Shoot()
    {
        for (int i = 0; i < shellPerRound; i++)
        {
            tempProjCreated = Instantiate(projectile, transform.position, transform.rotation,
                ProjectileManager.Instance.projectileParent);
        
            tempProjCreated.InitializeBaseStats(damage, projectileSpeed, projectileLifetime);
            ((PierceProjectile)tempProjCreated).SetPierce(piercePower);
            
            
            float spread = Random.Range(-weaponSpread, weaponSpread);
            Vector3 rot = tempProjCreated.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x,rot.y ,rot.z + spread);
            tempProjCreated.transform.rotation = Quaternion.Euler(rot);
        }
    }
}
