using System;
using UnityEngine;
using TMPro;
using CameraPath;

sealed class CameraPathFactory {
    public static GameObject PathFromPositionToInspection(Transform current, Transform inspectionTarget, float distance) {
        var pathGO = new GameObject("_GeneratedPath");
        CPC_CameraPath path = pathGO.AddComponent<CPC_CameraPath>();

        path.points.Add(new CPC_Point(current.position, current.rotation));

        Vector3 animationTargetPosition = inspectionTarget.position + (current.position - inspectionTarget.position).normalized * distance;
        Quaternion targetOrientation = Quaternion.LookRotation(inspectionTarget.position - animationTargetPosition, Vector3.up);
        path.points.Add(new CPC_Point(animationTargetPosition, targetOrientation));
        
        return pathGO;
    }
}