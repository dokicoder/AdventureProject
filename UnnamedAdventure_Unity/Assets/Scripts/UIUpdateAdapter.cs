using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class UIUpdateAdapter : MonoBehaviour
{
    public DialogueRunner.StringUnityEvent onLineUpdate;

    public DialogueRunner.StringUnityEvent onSpeakerUpdate;

    public void LineUpdateAdapter(string line) {
        if(line.Contains(":")) {
            // TODO: what if I want to use a colon in dialogue, dummy?
            string[] lineComponents = line.Split(new[] { ':' }, 2);
        
            if(lineComponents.Length >= 2) {
                onSpeakerUpdate?.Invoke(lineComponents[0].TrimStart());
                onLineUpdate?.Invoke(lineComponents[1].TrimStart());
            }
        }
        else {
             onLineUpdate?.Invoke(line);
        }
    }
}
