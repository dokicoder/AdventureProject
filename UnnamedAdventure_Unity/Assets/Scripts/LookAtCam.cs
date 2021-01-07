using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    void LateUpdate()
    {
        // TODO
       //MakeSpriteFaceCamera();
    }

    void MakeSpriteFaceCamera() {
        //Variant A
        //transform.LookAt(Camera.main.transform.position, Vector3.up);
        //Variant B
        //transform.forward = -Camera.main.transform.forward;
    }
}