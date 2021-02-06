using System;
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

    private bool userRequestedNextLine = false;

    // SkipLine will on first trigger, finish the current line animation (if one is running)
    // On second trigger it will indicate that the line is completed and the next is requested by triggering the onLineCompleted event (which should request the next line somehow, probably using Yarn's DialogueUI script)
    public void SkipLine() {
        Debug.LogWarning(textJuicer.IsPlaying);

        if(textJuicer.IsPlaying) {
            textJuicer.Complete();
            textJuicer.Stop();
        }
        else {
            onLineCompleted?.Invoke();
        }
    }

    void Start() {
        NPCs = new List<NPCController> ( FindObjectsOfType<NPCController> () );
    }


    public void LineUpdateAdapter(string line) {
        if(line.Contains(":")) {
            // splitting in just two parts allows using a colon ':' in the normal text as well
            string[] lineComponents = line.Split(new[] { ':' }, 2);
        
		   

            if(lineComponents.Length == 2) {
                // TODO: use this to add delay after line ends and by this give more emphasis and structure to the sentences
                string pattern = @"[^\.?!]+([\.]+|[?!]+)";
                Regex rgx = new Regex(pattern);
      
                foreach (Match match in rgx.Matches(lineComponents[1])) {
                    Debug.Log(String.Format("Found '{0}' at position {1}", match.Value.Trim(), match.Index));  
                }

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
