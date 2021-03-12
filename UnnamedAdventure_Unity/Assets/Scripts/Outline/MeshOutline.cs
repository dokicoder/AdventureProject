using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

[RequireComponent(typeof(Renderer))]
[DisallowMultipleComponent]
public class MeshOutline : MonoBehaviour
{
    [SerializeField]
    [Range( 0.0f, 1.0f )]
    private float size = 0.015f;
    public float Size {
        get { return size; }
        set {
          size = value;
          OutlineRenderer.sharedMaterial.SetFloat( "_OutlineAmount", value ); 
        }
    }

    public bool Enabled {
        get { return OutlineRenderer.sharedMaterial.GetInt("_Enabled") == 1; }
        set {  OutlineRenderer.sharedMaterial.SetInt( "_Enabled", value ? 1 : 0 ); }
    }

    public Color Color {
        get { return OutlineRenderer.sharedMaterial.GetColor("_OutlineColor"); }
        set {  OutlineRenderer.sharedMaterial.SetColor( "_OutlineColor", value ); }
    }

    private Renderer outlineRenderer;
    private Renderer OutlineRenderer
    {
        get
        {
            if (outlineRenderer == null)
            {
              outlineRenderer = GetComponent<Renderer>();
            }
            return outlineRenderer;
        }
    }
    private void OnValidate()
    {
        Size = size;
    }
}