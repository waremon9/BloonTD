using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePopCallBack : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        EnemiesManager.Instance.ParticlePopBackToPool(gameObject);
    }
}
