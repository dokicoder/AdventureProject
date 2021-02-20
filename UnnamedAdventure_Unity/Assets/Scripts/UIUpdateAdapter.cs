using System;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;
using BrunoMikoski.TextJuicer;

public class UIUpdateAdapter : MonoBehaviour
{
    public DialogueRunner.StringUnityEvent onLineUpdate;

    public DialogueRunner.StringUnityEvent onSpeakerUpdate;

    // TODO: Dear Dev (aka me), your naming sucks balls. Regards, yourself from the future
    public TextMeshProUGUI SpeakerText;
    public TextMeshProUGUI TextText;

    private List<NPCController> NPCs; 

    /// <summary>
    /// This event is triggered after the whole line is completed (nut not after just one of the sentences is complete)
    /// </summary>
    public UnityEngine.Events.UnityEvent onLineCompleted;

    [SerializeField]
    private TMP_TextJuicer textJuicer;

    private Boolean _autoContinueAfterLine = false;

    // SkipLine will on first trigger, finish the current line animation (if one is running)
    // On second trigger it will indicate that the line is completed and the next is requested by triggering the onLineCompleted event (which should request the next line somehow, probably using Yarn's DialogueUI script)
    public void SkipLine() {
        if(textJuicer.IsPlaying) {
            textJuicer.Complete();
            textJuicer.Stop();
        }
        else {
            onLineCompleted?.Invoke();
        }
    }

    void Update() {
        if(_autoContinueAfterLine && !textJuicer.IsPlaying) {
            onLineCompleted?.Invoke();
            _autoContinueAfterLine = false;
        }
    }

    void Start() {
        NPCs = new List<NPCController> ( FindObjectsOfType<NPCController> () );
    }

    private (string name, string text) SplitLine(string line) {
        string[] lineComponents = line.Split(new[] { ':' }, 2);

        return (lineComponents[0], lineComponents[1]);
    }

    public void LineUpdateAdapter(string line) {
        if(line.Contains(">>>")) {
            line = line.Replace(">>>", "");

            _autoContinueAfterLine = true;
        }

        if(line.Contains(":")) {
            // split the next line string in two parts: the name label of the character reading [0], and the normal text [1] along a colon ':'
            // IMPORTANT: The normal text may contain colons ':', the character name however MUST NOT 
            (string name, string textWithWhitespace) = SplitLine(line);
            string text = textWithWhitespace.TrimStart();
        
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
    
            onLineUpdate?.Invoke(text);
        }
        else {
            onLineUpdate?.Invoke(line);
        }
    }
}
