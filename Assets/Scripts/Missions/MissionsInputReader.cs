using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionsInputReader : MonoBehaviour, Controls.IMissionActions
{
    Controls _inputActions;
    Missions _missions;

    private void OnEnable()
    {
        _missions = FindObjectOfType<Missions>(); // must be single
        if (_inputActions != null)
        {
            return;
        }
        _inputActions = new Controls();
        _inputActions.Mission.SetCallbacks(this);
        _inputActions.Mission.Enable();
    }
    private void OnDisable()
    {
        _inputActions.Mission.Disable();
    }
    public void OnSendAnswer(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _missions.HandleAnswer();
        }
    }
}
