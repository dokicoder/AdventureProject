using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCController : MonoBehaviour
{
    [Header("Optional")]
    public YarnProgram scriptToLoad;

    public string characterName = "";
    public string talkToNode = "";
    
    private Color _stored;

    private DialogueRunner _dialogRunner;

    // Start is called before the first frame update
    void Start()
    {
        _dialogRunner = FindObjectOfType<DialogueRunner>();

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
        if (_dialogRunner.IsDialogueRunning == true) {
            return;
        }

        try {
            // Kick off the dialogue at this node.
            _dialogRunner.StartDialogue (talkToNode);
            SetHighlighted(false);
            ShowName(false);
        } catch( Exception e ) {
            Debug.LogWarningFormat("Could not start dialogue on node {0}", talkToNode);
        }
    }
    void OnMouseEnter() {
        if (_dialogRunner.IsDialogueRunning == true) {
            return;
        }

        SetHighlighted(true);
        ShowName(true);
    }

    void OnMouseExit() {
        if (_dialogRunner.IsDialogueRunning == true) {
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
                GameObject outline = transform.Find("Outline").gameObject;
                outline.SetActive(true);
            } catch( NullReferenceException e ) {}
        }
        else {
            try {
            Material m = GetComponent<MeshRenderer>()?.material;
            m.color = _stored;
            } catch(MissingComponentException e) {}

            try {
                GameObject outline = transform.Find("Outline").gameObject;
                outline.SetActive(false);
            } catch(NullReferenceException e) {}
        }
    }

    void ShowName(Boolean show) {
        try {
            GameObject nameCanvas = transform.Find("Name").gameObject;
            nameCanvas.SetActive(show);
        } catch( NullReferenceException e ) {}
    }
}
