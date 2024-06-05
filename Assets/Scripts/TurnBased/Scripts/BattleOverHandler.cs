using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using ModestTree;

public class BattleOverHandler : MonoBehaviour
{
    [SerializeField] GameObject _battleCanvas;
    [SerializeField] GameObject _postBattleCanvas;
    void Start()
    {
        _battleCanvas.SetActive(true);
        _postBattleCanvas.SetActive(false);
    }

    public void ChangeToBattleEnd(List<BattleItemSO> items)
    {
        if (items.IsEmpty())
        {
            Debug.Log("empty");
        }
        _battleCanvas.SetActive(false);
        _postBattleCanvas.SetActive(true);

    }

    public void ExitCombatScreen()
    {
        SceneManager.LoadScene("Finale", LoadSceneMode.Single);
    }
}
