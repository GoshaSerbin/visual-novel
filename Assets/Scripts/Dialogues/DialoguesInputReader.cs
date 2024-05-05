using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguesInputReader : MonoBehaviour, Controls.IDialogueActions
{
    Controls _inputActions;
    AIManager _aiManager;
    private void OnEnable()
    {

        _aiManager = FindObjectOfType<AIManager>();
        if (_inputActions != null)
        {
            return;
        }
        _inputActions = new Controls();
        _inputActions.Dialogue.SetCallbacks(this);
        _inputActions.Dialogue.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Dialogue.Disable();
    }
    public void OnNextPhrase(InputAction.CallbackContext context)
    {
        // if (context.started && _dialogues.IsPrewrittenDialoguePlay)
        // {
        //     // _dialogues.ContinueStory();
        // }
    }
    public void OnSendPhrase(InputAction.CallbackContext context)
    {
        // if (context.started && _aiManager.isAITalking)
        // {
        //     Debug.Log("Send phrase!");
        //     // _dialogues.ContinueAITalk();
        //     // send request to server
        // }
    }
}