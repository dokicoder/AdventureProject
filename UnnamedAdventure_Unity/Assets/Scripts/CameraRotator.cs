using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRotator : MonoBehaviour
{
    public Transform targetTransform;

    public float rotationSpeed = 2.0f;

    public float minDistance = 2.0f;
     public float maxDistance = 10.0f;

    private Vector3 lastOrientationVector;

    // a vector representing the camera rotation for the current screen space mouse position
    // this is calculate every frame to get a "delta rotation" for the camera update
    private Vector3 GetMouseDirectionVector(Camera cam) {
        // the constant last parameter should be the camera's near clip plane distance. using a constant makes the rotation speed independent from camera distance (which we want)
        Vector3 screenCoords = new Vector3( Input.mousePosition.x, Input.mousePosition.y, 5.0f );
        Vector3 screenPos = cam.ScreenToWorldPoint(screenCoords);

        return (screenPos - targetTransform.position).normalized;
    }

    void OnDrawGizmos()
	{
        Gizmos.color = Color.red;

		Gizmos.DrawLine(targetTransform.position, targetTransform.position + lastOrientationVector);
        Gizmos.DrawSphere(targetTransform.position, 0.03f);
        Gizmos.DrawSphere(targetTransform.position + lastOrientationVector, 0.03f);
	}

    // Update is called once per frame
    void Update()
    {
        Camera cam = GetComponent<Camera>();

        if ( Input.GetMouseButtonDown(0) ) {
            cam.transform.LookAt(targetTransform.position);
            lastOrientationVector = GetMouseDirectionVector(cam);
        }

        if ( Input.GetMouseButton(0) ) {
            Vector3 currentOrientationVector = GetMouseDirectionVector(cam);

            Quaternion deltaRot = Quaternion.FromToRotation(lastOrientationVector, currentOrientationVector);

            float angle = 0;
            Vector3 axis;
            deltaRot.ToAngleAxis(out angle, out axis);
            deltaRot = Quaternion.AngleAxis(angle * rotationSpeed, axis);

            // rotate the delta vector between camera and target, and add it to the target position: voila - the camera was rotated around the target
            cam.transform.position = targetTransform.position + deltaRot * (cam.transform.position - targetTransform.position);
       
            cam.transform.LookAt(targetTransform.position, Vector3.up);

            // we cannot reuse currentOrientationVector here since we rerotated and -oriented the camera. We have to calculate it again
            lastOrientationVector = GetMouseDirectionVector(cam);
        }

        float newCamDistance = (cam.transform.position - targetTransform.position).magnitude * (1 + -0.03f * Input.mouseScrollDelta.y);
        newCamDistance = Mathf.Clamp(newCamDistance, minDistance, maxDistance);

        cam.transform.position = targetTransform.position + ((cam.transform.position - targetTransform.position).normalized * newCamDistance);
    }
}
