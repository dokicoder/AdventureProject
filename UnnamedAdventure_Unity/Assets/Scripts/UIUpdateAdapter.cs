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

    [SerializeField]
    private float sentenceDelay = 0.7f;

    private bool userRequestedNextLine = false;
    private List<string> _activeSentenceList;

    private enum SentenceState
    {
        TRIGGERED,
        RUNNING,
        PENDING,
    }

    private SentenceState _sentenceState = SentenceState.PENDING;

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
    private void StartSentence() {
         _sentenceState = SentenceState.RUNNING;
        // TODO: if you really are hard-pressed to find things to do (won't happen), worry about complexity here and use the appropriate data structure (stack I suppose)
        onLineUpdate?.Invoke(_activeSentenceList.First());
        _activeSentenceList.RemoveAt(0);
    }

    private IEnumerator StartSentenceDelayed() {
        yield return new WaitForSeconds(sentenceDelay);
        StartSentence();
    }

    void Update() {
        // after playing has finished, sentence state is pending again
        if(_sentenceState == SentenceState.RUNNING && textJuicer.IsPlaying) {
            _sentenceState = SentenceState.PENDING;
        }
        
        if(_sentenceState == SentenceState.PENDING && _activeSentenceList?.Count > 0) {
            _sentenceState = SentenceState.TRIGGERED;
            StartCoroutine(StartSentenceDelayed());
        }
    }

    private (string name, string text) SplitLine(string line) {
        string[] lineComponents = line.Split(new[] { ':' }, 2);

        return (lineComponents[0], lineComponents[1]);
    }
    private List<string> SplitToSentences(string text) {
        string pattern = @"[^\.?!]+([\.]+|[?!]+)";
        
        // IMPORTANT: this does not strip the leading whitespace from the sentences (we should not, because they would end up missing in the final displayed text)
        // I guess this just introduces a constant minimal delay, but better to keep in mind
        return new Regex(pattern)
            .Matches(text)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();
    }

    public void LineUpdateAdapter(string line) {
        if(line.Contains(":")) {
            // split the next line string in two parts: the name label of the character reading [0], and the normal text [1] along a colon ':'
            // IMPORTANT: The normal text may contain colons ':', the character name however MUST NOT 
            (string name, string text) = SplitLine(line);
        
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
    
            _activeSentenceList = SplitToSentences(text);
        }
        else {
            _activeSentenceList = SplitToSentences(line);
        }
    }
}
