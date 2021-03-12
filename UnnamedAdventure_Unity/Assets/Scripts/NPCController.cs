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
    private Boolean _hovered = false;

    private SelectionLabelController labelController;
    private SelectionLabelController LabelController
    {
        get
        {
            if (labelController == null)
            {
                labelController = GameObject.FindWithTag("GameController").GetComponent<SelectionLabelController>();
            }
            return labelController;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Outline.Color = Color;

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

        LabelController.RequestOwnership(gameObject);

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        _hovered = false;

        LabelController.SubmitOwnership(gameObject);

        SetHighlighted(false);
        ShowName(false);
    }

    void SetHighlighted(Boolean highlighted) {
        Outline.Enabled = highlighted;
    }

    void UpdateLabelPosition() {
        var renderer = Outline.GetComponent<SpriteRenderer>();
        Vector3 updatedPos = SelectionLabelController.CalculateLabelScreenPosition(renderer, TopOffset);

        LabelController.UpdateLabelPosition(gameObject, updatedPos);
    }

    void ShowName(Boolean show) {
        if(show) {
            LabelController.UpdateLabelVisibility(gameObject, true);
            LabelController.UpdateLabelText(gameObject, Name, Color);
        } else {
            LabelController.UpdateLabelVisibility(gameObject, false);
        }
    }
}
