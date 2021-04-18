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

    public bool Selectable = true;

    public UnityEngine.Events.UnityEvent<YarnProgram> onRegisterDialogue;
    public UnityEngine.Events.UnityEvent<string> onStartDialogue;

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

        onRegisterDialogue?.Invoke(scriptToLoad);
    }

    void Update() {
        if(_hovered) {
            UpdateLabelPosition();
        }
    }

    void OnMouseDown() {
        // Don't trigger when in dialog control when non-selectable - e.g. when already in dialogue (controlled by GameStateManager)
        if (!Selectable) {
            return;
        }
        
        // Kick off the dialogue at this node.
        onStartDialogue?.Invoke(talkToNode);
        SetHighlighted(false);
        ShowName(false);
    }
    void OnMouseEnter() {
        // Don't trigger when in dialog control when non-selectable - e.g. when already in dialogue (controlled by GameStateManager)
        if (!Selectable) {
            return;
        }

        _hovered = true;

        LabelController.RequestOwnership(gameObject);

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        // Don't trigger when in dialog control when non-selectable - e.g. when already in dialogue (controlled by GameStateManager)
        if (!Selectable) {
            return;
        }

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
