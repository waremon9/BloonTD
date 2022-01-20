using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public void OnNextWaveClicke()
    {
        EnemiesManager.Instance.CallNextWave();
    }
}
