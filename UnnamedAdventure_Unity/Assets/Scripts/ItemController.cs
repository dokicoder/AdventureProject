using System;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class ItemController : MonoBehaviour
{
    public string Name = "";
    public Color Color;
    public Vector3 TopOffset = new Vector3(0, 30, 0);
    private GameObject _nameLabel;
    private DialogueRunner _dialogueRunner;

    private Boolean _hovered = false;

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

    // Start is called before the first frame update
    void Start()
    {
        Outline.Color = Color;

        _dialogueRunner = FindObjectOfType<DialogueRunner>();

        _nameLabel = GameObject.FindWithTag("NameLabel");

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
        var renderer = Outline.GetComponent<Renderer>();

        // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
        float halfBoundsHeight = renderer.bounds.size.y * renderer.transform.lossyScale.y * 0.5f;

        Vector3 meshTopScreenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, halfBoundsHeight, 0));
        _nameLabel.GetComponent<RectTransform>().position = meshTopScreenPosition + TopOffset;
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
