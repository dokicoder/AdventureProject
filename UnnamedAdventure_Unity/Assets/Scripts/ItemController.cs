using System;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class ItemController : MonoBehaviour
{
    public string Name = "";
    public Color Color;
    public Vector3 TopOffset = new Vector3(0, 30, 0);
    private DialogueRunner _dialogueRunner;

    private Boolean _hovered = false;
    [SerializeField]
    private MeshOutline meshOutline;
    private MeshOutline Outline
    {
        get
        {
            if (meshOutline == null)
            {
                meshOutline = GetComponent<MeshOutline>();
                if (meshOutline == null)
                    meshOutline = GetComponentInChildren<MeshOutline>();
            }
            return meshOutline;
        }
    }

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
        _dialogueRunner = FindObjectOfType<DialogueRunner>();

        Outline.Color = Color;
        SetHighlighted(false);
    }

    void Update() {
        if(_hovered) {
            UpdateLabelPosition();
        }
    }

    void OnMouseEnter() {
        // Don't trigger when in dialog control when we're in dialogue
        if (_dialogueRunner.IsDialogueRunning == true) {
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
        var renderer = Outline.GetComponent<Renderer>();
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
