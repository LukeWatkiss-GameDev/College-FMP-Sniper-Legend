using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (EnemyFOV))]
public class FovEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.enemyRange);
        Vector3 viewAngleaA = fov.DirFromAngle(-fov.fovAngle / 2, false);
        Vector3 viewAngleaB = fov.DirFromAngle(fov.fovAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleaA * fov.fovAngle);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleaB * fov.fovAngle);

        Handles.color = Color.red;
        foreach(Transform visableTarget in fov.visableTargets)
        {
            Handles.DrawLine(fov.transform.position, visableTarget.position);
        }
    } 
}
