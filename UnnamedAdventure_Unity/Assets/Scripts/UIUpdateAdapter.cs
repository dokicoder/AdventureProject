using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class UIUpdateAdapter : MonoBehaviour
{
    public DialogueRunner.StringUnityEvent onLineUpdate;

    public DialogueRunner.StringUnityEvent onSpeakerUpdate;

    // TODO: Dear Dev (aka me), your naming sucks balls. Regards, yourself from the future
    public TextMeshProUGUI SpeakerText;
    public TextMeshProUGUI TextText;

    private List<NPCController> NPCs; 

    void Start() {
        NPCs = new List<NPCController> ( FindObjectsOfType<NPCController> () );
    }

    public void LineUpdateAdapter(string line) {
        Debug.Log(line);

        if(line.Contains(":")) {
            // TODO: what if I want to use a colon in dialogue, dummy?
            string[] lineComponents = line.Split(new[] { ':' }, 2);
        
            if(lineComponents.Length >= 2) {
                string name = lineComponents[0].TrimStart();
                onSpeakerUpdate?.Invoke(name);
                if(name == "Player") {
                    SpeakerText.color = Color.white;
                    TextText.color = Color.white;
                }
                try {
                    var npcWithMatchingName = NPCs.Find(npc => npc.Name == name);
                    SpeakerText.color = npcWithMatchingName.Color;
                    TextText.color = npcWithMatchingName.Color;
                } catch(Exception e) {}
        
                onLineUpdate?.Invoke(lineComponents[1].TrimStart());
            }
        }
        else {
             onLineUpdate?.Invoke(line);
        }
    }
}
