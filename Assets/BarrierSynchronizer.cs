using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSynchronizer
{


    public static event Action OnWaitStarted;
    public static event Action OnWaitEnded;
    private HashSet<string> _processingVariables = new HashSet<string>();
    private HashSet<string> _blockingVariables = new HashSet<string>();

    public void AddProcessingVariable(string varName)
    {
        Debug.Log("BARRIER: added proc var " + varName);
        _processingVariables.Add(varName);
    }
    public void RemoveProcessingVariable(string varName)
    {
        Debug.Log("BARRIER: removed proc var " + varName);
        _processingVariables.Remove(varName);
        if (_blockingVariables.Contains(varName))
        {
            Debug.Log("BARRIER: this var was blocking ");
            _blockingVariables.Remove(varName);
            if (_blockingVariables.Count == 0)
            {
                Debug.Log("BARRIER: will invoke waitEnded");
                OnWaitEnded?.Invoke();
            }
        }
    }

    public void AddBlockingVariable(string varName)
    {
        if (_processingVariables.Contains(varName))
        {
            _blockingVariables.Add(varName);
            Debug.Log("BARRIER: added block var " + varName);
        }
        else
        {
            Debug.Log("var " + varName + "is already set or never has been processed");
        }
    }

    public bool IsBlocked()
    {
        return _blockingVariables.Count > 0;
    }
    public void Barrier()
    {
        if (_blockingVariables.Count == 0)
        {
            // it is better to first check this by IsBlocked to not do unnessecery movements
            Debug.LogWarning("No blocking variables currently");
            OnWaitStarted?.Invoke();
            OnWaitEnded?.Invoke();
            return;
        }
        Debug.Log("BARRIER: will invoke waitStarted");
        OnWaitStarted?.Invoke();
    }
}
