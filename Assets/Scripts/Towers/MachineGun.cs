using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MachineGun : PierceTower
{
    [SerializeField] protected float weaponSpread;

    public override void Shoot()
    {
        base.Shoot();
        
        float spread = Random.Range(-weaponSpread, weaponSpread);
        Vector3 rot = tempProjCreated.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x,rot.y ,rot.z + spread);
        tempProjCreated.transform.rotation = Quaternion.Euler(rot);
    }
}
