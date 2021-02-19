using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * 0.07f * Time.deltaTime);
        transform.Rotate(transform.right, 0.06f * Time.deltaTime);
    }
}
