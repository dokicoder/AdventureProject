using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

public class NPCController : MonoBehaviour
{
    [Header("Optional")]
    public YarnProgram scriptToLoad;
    // Custom top offset for the name label
    public Vector3 TopOffset = new Vector3(0, 30, 0);

    public Color Color;

    public string Name = "";
    public string talkToNode = "";
    
    private Color _stored;

    private DialogueRunner _dialogueRunner;
    private GameObject _nameLabel;

    void CreateOutline() {
        GameObject outline = new GameObject("_Outline");

        outline.transform.parent = transform;
        outline.transform.localPosition = Vector3.zero;
        outline.SetActive(false);

        Material outlineMaterial = new Material( Shader.Find("Sprites/OutlineShader") );
        outlineMaterial.SetColor("_OutlineColor", Color);

        const int num_outlines = 7;
        float angle = 0;
        for( int i = 0; i < num_outlines; ++i ) {
            GameObject outlinePart = new GameObject("_OutlinePart");
            outlinePart.AddComponent<RectTransform>();
            var spriteRenderer = outlinePart.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.material = outlineMaterial;
            outlinePart.transform.parent =  outline.transform;
            outlinePart.transform.localScale = transform.localScale;
            outlinePart.transform.localPosition = new Vector3(Mathf.Cos(angle) * 0.01f, -Mathf.Sin(angle) * 0.01f, 0);

            angle += 2 * Mathf.PI / num_outlines;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: for this, canvas and label need to be active on startup. Not sure if this is good
        _nameLabel = GameObject.FindWithTag("NameLabel");
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        
        // the NPC might have a mesh renderr - if it does, this is a 3D NPC and we don't need to bother creating the outline
        try {
            CreateOutline();
        } catch( Exception e ) {}

        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);                
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        // Don't trigger when in dialog control when we're in dialogue
        if (_dialogueRunner.IsDialogueRunning == true) {
            return;
        }

        try {
            // Kick off the dialogue at this node.
            _dialogueRunner.StartDialogue (talkToNode);
            SetHighlighted(false);
            ShowName(false);
        } catch( Exception e ) {
            Debug.LogWarningFormat("Could not start dialogue on node {0}", talkToNode);
        }
    }
    void OnMouseEnter() {
        if (_dialogueRunner?.IsDialogueRunning == true) {
            return;
        }

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        if (_dialogueRunner?.IsDialogueRunning == true) {
            return;
        }

        SetHighlighted(false);
        ShowName(false);
    }

    void SetHighlighted(Boolean highlighted) {
        if(highlighted) {
            try {
                Material m = GetComponent<MeshRenderer>()?.material;
                _stored = m.color;
                m.color = new Color(1.0f, 0f, 0f);
            } catch( MissingComponentException e ) {}

            try {
                GameObject outline = transform.Find("_Outline").gameObject;
                outline.SetActive(true);
            } catch( NullReferenceException e ) {}
        }
        else {
            try {
            Material m = GetComponent<MeshRenderer>()?.material;
            m.color = _stored;
            } catch(MissingComponentException e) {}

            try {
                GameObject outline = transform.Find("_Outline").gameObject;
                outline.SetActive(false);
            } catch(NullReferenceException e) {}
        }
    }

    void ShowName(Boolean show) {
        Component[] components = _nameLabel.GetComponents(typeof(Component));
        foreach(Component component in components) {
            Debug.Log(component.ToString());
        }

        if(show) {
            _nameLabel.SetActive(true);

            // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
            float halfSpriteHeight = GetComponent<SpriteRenderer>().sprite.bounds.size.y * transform.localScale.y * 0.5f;

            Vector3 spriteTopScreenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, halfSpriteHeight, 0));
            _nameLabel.GetComponent<RectTransform>().position = spriteTopScreenPosition + TopOffset;

            _nameLabel.GetComponent<TextMeshProUGUI>().color = Color;
            _nameLabel.GetComponent<TextMeshProUGUI>().text = Name;
        } else {
            _nameLabel.SetActive(false);
        }
    }
}
