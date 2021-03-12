using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class SpriteOutline : MonoBehaviour
{
    // TODO: this whole approach of multiple sprites is slow. Improve on it if there is time
    private const int NUM_OUTLINES = 10;

    public Color Color {
        set { CreateOutline(value); }
    }

    [Range( 0.0f, 1.0f )]
    public float Size = 0.01f;

    private GameObject outlineGO;
    private GameObject OutlineGO {
        get
        {
            if (outlineGO == null)
            {
                CreateOutline(Color.black);
            }
            return outlineGO;
        }
    }

    public bool Enabled
    {
        get { return OutlineGO.activeSelf; }
        set { OutlineGO.SetActive(value); }
    }

    private void CreateOutline(Color Color) {
        outlineGO = new GameObject("_Outline");

        outlineGO.transform.parent = transform;
        outlineGO.transform.localPosition = Vector3.zero;
        outlineGO.SetActive(false);

        Material outlineMaterial = new Material( Shader.Find("Sprites/OutlineShader") );
        outlineMaterial.SetColor("_OutlineColor", Color);

        // outline is created by adding multiple sprites with plain color and offsetting them circular in all directions
        float angle = 0;
        for( int i = 0; i < NUM_OUTLINES; ++i ) {
            GameObject outlinePart = new GameObject("_OutlinePart");
            outlinePart.AddComponent<RectTransform>();
            
            var spriteRenderer = outlinePart.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.material = outlineMaterial;

            outlinePart.transform.SetParent( outlineGO.transform );
            outlinePart.transform.localScale = transform.localScale;
            outlinePart.transform.localPosition = new Vector3(Mathf.Cos(angle) * -Size, Mathf.Sin(angle) * -Size, 0);

            angle += 2 * Mathf.PI / NUM_OUTLINES;
        }
    }
}
