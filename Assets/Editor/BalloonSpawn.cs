using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicBalloonScriptable))]
public class BalloonSpawn : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("spawn balloon"))
            {
                EnemiesManager.Instance.EnemieSpawnAtStart((BasicBalloonScriptable)target);
            }
        }
    }
}
