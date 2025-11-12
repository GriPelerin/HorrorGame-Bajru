using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterVision))]
public class MonsterVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        MonsterVision monsterVision = (MonsterVision)target;

        Handles.color = Color.yellow;
        Handles.DrawWireArc(monsterVision.transform.position, Vector3.up, Vector3.forward, 360, monsterVision.ViewRadius);

        Vector3 viewAngleA = monsterVision.DirFromAngle(-monsterVision.ViewAngle / 2, false);
        Vector3 viewAngleB = monsterVision.DirFromAngle(monsterVision.ViewAngle / 2, false);

        Handles.color = Color.magenta;
        Handles.DrawLine(monsterVision.transform.position, monsterVision.transform.position + viewAngleA * monsterVision.ViewRadius);
        Handles.DrawLine(monsterVision.transform.position, monsterVision.transform.position + viewAngleB * monsterVision.ViewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in monsterVision.visibleTargets)
        {
            Handles.DrawLine(monsterVision.transform.position, visibleTarget.position);
        }

    }
}
