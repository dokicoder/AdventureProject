using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;

[DisallowMultipleComponent]
public class NPCController : MonoBehaviour
{
    [Header("Optional")]
    public YarnProgram scriptToLoad;
    // Custom top offset for the name label
    public Vector3 TopOffset = new Vector3(0, 30, 0);

    public Color Color;

    public string Name = "";
    public string talkToNode = "";

    [SerializeField]
    private SpriteOutline spriteOutline;
    private SpriteOutline Outline
    {
        get
        {
            if (spriteOutline == null)
            {
                spriteOutline = GetComponent<SpriteOutline>();
                if (spriteOutline == null)
                    spriteOutline = GetComponentInChildren<SpriteOutline>();
            }
            return spriteOutline;
        }
    }

    private DialogueRunner _dialogueRunner;
    private GameObject _nameLabel;

    private Boolean _hovered = false;

    // Start is called before the first frame update
    void Start()
    {
        Outline.Color = Color;

        // TODO: for this, canvas and label need to be active on startup. Not sure if this is good
        _nameLabel = GameObject.FindWithTag("NameLabel");
        _dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);                
        }

    }

    void Update() {
        if(_hovered) {
            UpdateLabelPosition();
        }
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
        } catch( Exception ) {
            Debug.LogWarningFormat("Could not start dialogue on node {0}", talkToNode);
        }
    }
    void OnMouseEnter() {
        if (_dialogueRunner?.IsDialogueRunning == true) {
            return;
        }

        _hovered = true;

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        _hovered = false;

        SetHighlighted(false);
        ShowName(false);
    }

    void SetHighlighted(Boolean highlighted) {
        Outline.Enabled = highlighted;
    }

    void UpdateLabelPosition() {
         // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
        float halfSpriteHeight = Outline.GetComponent<SpriteRenderer>().sprite.bounds.size.y * transform.localScale.y * 0.5f;

        Vector3 spriteTopScreenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, halfSpriteHeight, 0));
        _nameLabel.GetComponent<RectTransform>().position = spriteTopScreenPosition + TopOffset;
    }

    void ShowName(Boolean show) {
        if(show) {
            _nameLabel.SetActive(true);

            UpdateLabelPosition();

            _nameLabel.GetComponent<TextMeshProUGUI>().color = Color;
            _nameLabel.GetComponent<TextMeshProUGUI>().text = Name;
        } else {
            _nameLabel.SetActive(false);
        }
    }
}
