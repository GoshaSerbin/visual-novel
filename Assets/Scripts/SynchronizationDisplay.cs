using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizationDisplay : MonoBehaviour
{


    [SerializeField] private GameObject _loadingIcon;
    [SerializeField] private GameObject _nextPhraseButton;

    void OnEnable()
    {
        BarrierSynchronizer.OnWaitStarted += StartWait;
        BarrierSynchronizer.OnWaitEnded += EndWait;
    }
    void OnDisable()
    {
        BarrierSynchronizer.OnWaitStarted -= StartWait;
        BarrierSynchronizer.OnWaitEnded -= EndWait;
    }

    void StartWait()
    {
        _loadingIcon.SetActive(true);
    }
    void EndWait()
    {
        _loadingIcon.SetActive(false);
    }
}
