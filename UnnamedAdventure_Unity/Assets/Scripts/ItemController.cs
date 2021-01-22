using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string Name = "";
    public Color Color;
    public Renderer MeshRenderer;
    private Material _outlineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _outlineMaterial = MeshRenderer?.material; 

        try {
            UnityEngine.UI.Text nameLabelText = transform.Find("Name/Label")?.GetComponent<UnityEngine.UI.Text>();
            nameLabelText.color = Color;
            nameLabelText.text = Name;

            _outlineMaterial.SetColor("_OutlineColor", Color);      
        } catch(Exception e) {}

        SetHighlighted(false);
        ShowName(false);
    }

    void OnMouseEnter() {
        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        SetHighlighted(false);
        ShowName(false);
    }

    void SetHighlighted(Boolean highlighted) {
        try {
            _outlineMaterial.SetFloat("_OutlineAmount", highlighted ? 0.016f : 0f);  
        } catch( Exception e ) {}
    }

    void ShowName(Boolean show) {
        try {
            GameObject nameCanvas = transform.Find("Name").gameObject;
            nameCanvas.SetActive(show);
        } catch( NullReferenceException e ) {}
    }
}
