using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoProjectileTower : BaseTower
{
    [Header("Damage")]
    [SerializeField] protected int damage;

    [Header("Attack type")]
    [SerializeField] protected ProjectileType type;
}
