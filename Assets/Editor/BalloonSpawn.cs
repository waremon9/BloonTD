using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BalloonScriptable))]
public class BalloonSpawn : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("spawn balloon"))
            {
                EnemiesManager.Instance.EnemieSpawnAtStart((BalloonScriptable)target);
            }
        }
    }
}
