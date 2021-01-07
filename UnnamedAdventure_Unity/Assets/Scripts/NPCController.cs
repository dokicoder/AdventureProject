using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    Color _stored;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        Debug.Log("Clicked");
    }

    void OnMouseEnter() {
        try {
            Material m = GetComponent<MeshRenderer>()?.material;
            _stored = m.color;
            m.color = new Color(1.0f, 0f, 0f);
        } catch(MissingComponentException e) {}

        try {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            _stored = s.color;
            s.color = new Color(1.0f, 0f, 0f);
        } catch(MissingComponentException e) {}
    }

    void OnMouseExit() {
        try {
            Material m = GetComponent<MeshRenderer>()?.material;
            m.color = _stored;
        } catch(MissingComponentException e) {}

        try {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            s.color = _stored;
        } catch(MissingComponentException e) {}
    }
}
