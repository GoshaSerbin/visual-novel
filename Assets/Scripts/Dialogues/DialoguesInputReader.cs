using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialoguesInputReader : MonoBehaviour, Controls.IDialogueActions
{
    Controls _inputActions;
    Dialogues _dialogues;
    private void OnEnable()
    {
        _dialogues = FindObjectOfType<Dialogues>();
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
        if (context.started && _dialogues.DialogPlay)
        {
            _dialogues.ContinueStory(_dialogues._choiceButtonsPanel.activeInHierarchy); // странновато
        }
    }
    public void OnSendPhrase(InputAction.CallbackContext context)
    {
        if (context.started && !_dialogues.DialogPlay)
        {
            Debug.Log("Send phrase!");
            _dialogues.ContinueAITalk();
            // send request to server
        }
    }
}