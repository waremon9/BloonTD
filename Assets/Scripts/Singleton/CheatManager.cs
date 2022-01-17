using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MySingleton<CheatManager>
{
    public override bool DoDestroyOnLoad { get; }

    public BalloonScriptable balloon;
    public int qte;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartCoroutine(BalloonSpawnCoroutine());
        }
    }

    private IEnumerator BalloonSpawnCoroutine()
    {
        for (int i = 0; i < qte; i++)
        {
            EnemiesManager.Instance.EnemieSpawnAtStart(balloon);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
