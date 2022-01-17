using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MySingleton<CheatManager>
{
    public override bool DoDestroyOnLoad { get; }

    public BasicBalloonScriptable basicBalloon;
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
            EnemiesManager.Instance.EnemieSpawnAtStart(basicBalloon);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
