using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    void Start() 
    {
#if UNITY_EDITOR
            debugUI = Instantiate(debugUIPrefab);

            debugUI.transform.SetParent( GameObject.Find("Canvas").transform );
            debugUI.transform.position = new Vector3(20, Screen.height -20, 0);
#endif

        state = GameState.Overview;
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