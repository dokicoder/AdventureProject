using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CameraPath;
using Yarn.Unity;


public enum GameState {
    Dialog,
    ControlledCamera,
    Inspection,
    Overview,
    Menu
}

public class GameStateManager : MonoBehaviour
{   
    [SerializeField]
    private GameState state;

#if UNITY_EDITOR
    public GameObject debugUIPrefab;
    private GameObject debugUI;
#endif

    private DialogueRunner dialogueRunner;
    private DialogueRunner DialogueRunner
    {
        get
        {
            if (dialogueRunner == null)
            {
                dialogueRunner = GameObject.FindGameObjectWithTag("DialogueMain").GetComponent<DialogueRunner>();
            }
            return dialogueRunner;
        }
    }

    private DialogueUI dialogueUI;
    private DialogueUI DialogueUI
    {
        get
        {
            if (dialogueUI == null)
            {
                dialogueUI = GameObject.FindGameObjectWithTag("DialogueMain").GetComponent<DialogueUI>();
            }
            return dialogueUI;
        }
    }

     private NPCController[] npcs;
    private NPCController[] NPCs
    {
        get
        {
            if (npcs == null)
            {
                npcs = GameObject.FindObjectsOfType<NPCController>();
            }
            return npcs;
        }
    }

    private void HandleDialogueEnd() {
        foreach(NPCController npc in NPCs) {
            npc.Selectable = true;
        }
    }

    void TriggerDialogue(string talkToNode) {
        try {
            // Kick off the dialogue at this node.
            DialogueRunner.StartDialogue (talkToNode);
            state = GameState.Dialog;

            foreach(NPCController npc in NPCs) {
                npc.Selectable = false;
            }
        } catch( Exception ) {
            Debug.LogWarningFormat("Could not start dialogue on node {0}", talkToNode);
        }
    }

    void AddDialogue(YarnProgram dialogueScript) {
        DialogueRunner.Add(dialogueScript);   
    }

    void OnEnable() 
    {
#if UNITY_EDITOR
        debugUI = Instantiate(debugUIPrefab);

        debugUI.transform.SetParent( GameObject.Find("Canvas").transform );
        debugUI.transform.position = new Vector3(20, Screen.height -20, 0);
#endif
        state = GameState.Overview;

        foreach(NPCController npc in NPCs) {
            npc.onRegisterDialogue.AddListener(AddDialogue);
            npc.onStartDialogue.AddListener(TriggerDialogue);
        }

        DialogueUI.onDialogueEnd.AddListener(HandleDialogueEnd);

        //CPC_CameraPath path = CameraPathFactory.PathFromPositionToInspection(Camera.main.transform, GameObject.Find("DeadBody").transform, 1.7f).GetComponent<CPC_CameraPath>();
        //path.selectedCamera = Camera.main;
        //path.PlayPath(2.0f);
    }

    void Update() {
#if UNITY_EDITOR
        debugUI.transform.Find("Mode").GetComponent<TextMeshProUGUI>().text = string.Format("Mode: {0}", state);
#endif
    }

    void EnableSelectionMode() 
    {
        //TODO: implement
    }
}