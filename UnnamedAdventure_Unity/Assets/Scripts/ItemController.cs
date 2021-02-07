using System;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class ItemController : MonoBehaviour
{
    public string Name = "";
    public Color Color;
    public Vector3 TopOffset = new Vector3(0, 30, 0);
    private Material _outlineMaterial;
    private GameObject _nameLabel;
    private Renderer _renderer;
    private DialogueRunner _dialogueRunner;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueRunner = FindObjectOfType<DialogueRunner>();

        _renderer = GetComponentInChildren<Renderer>();

        _nameLabel = GameObject.FindWithTag("NameLabel");
        _outlineMaterial = _renderer?.material; 

        _outlineMaterial.SetColor("_OutlineColor", Color);      

        SetHighlighted(false);
    }

    void OnMouseEnter() {
        // Don't trigger when in dialog control when we're in dialogue
        if (_dialogueRunner.IsDialogueRunning == true) {
            return;
        }

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        SetHighlighted(false);
        ShowName(false);
    }

    void SetHighlighted(Boolean highlighted) {
        try {
            _outlineMaterial.SetInt("_Enabled", highlighted ? 1 : 0);
        } catch( Exception e ) {}
    }

     void ShowName(Boolean show) {
        if(show) {
            _nameLabel.SetActive(true);

            // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
            float halfBoundsHeight = _renderer.bounds.size.y * _renderer.transform.lossyScale.y * 0.5f;

            Vector3 meshTopScreenPosition = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, halfBoundsHeight, 0));
            _nameLabel.GetComponent<RectTransform>().position = meshTopScreenPosition + TopOffset;

            _nameLabel.GetComponent<TextMeshProUGUI>().color = Color;
            _nameLabel.GetComponent<TextMeshProUGUI>().text = Name;
        } else {
            _nameLabel.SetActive(false);
        }
    }
}
