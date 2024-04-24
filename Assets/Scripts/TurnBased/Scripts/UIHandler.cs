using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _enemyUIPrefab;
    [SerializeField] private Transform _parentPanel;

    [SerializeField] private TextMeshProUGUI _mainCharHealthText;
    [SerializeField] private Transform _mainCharHealthBar;
    private MainCharBattle _mainCharBattle;

    [SerializeField] private Dictionary<EnemyBattle, EnemyUIComponents> _enemyUIs = new();

    public void SpawnUIs(List<EnemyBattle> enemyData, MainCharBattle mainChar)
    {
        for (int i = 0; i < enemyData.Count; ++i)
        {
            var newUI = Instantiate(_enemyUIPrefab, _parentPanel);
            var newUIComps = newUI.GetComponent<EnemyUIComponents>();
            newUIComps.NameText.text = enemyData[i].GetEnemyName();
            _enemyUIs.Add(enemyData[i], newUIComps);
            UpdateHealth(enemyData[i]);
        }
        _mainCharBattle = mainChar;
        UpdateHealth(_mainCharBattle);
    }
    public void UpdateHealth(CharacterBattle character)
    {
        var currHealth = character.GetCurrentHealth();
        var maxHealth = character.GetMaxHealth();
        if (character == _mainCharBattle)
        {
            _mainCharHealthText.text = currHealth.ToString() + "/" + maxHealth.ToString();
            _mainCharHealthBar.localScale = new Vector3((float)(currHealth) / maxHealth, 1f, 1f);
            return;
        }
        EnemyBattle enemy = character as EnemyBattle;
        _enemyUIs[enemy].HealthText.text = currHealth.ToString() + "/" + maxHealth.ToString();
        _enemyUIs[enemy].HealthBar.localScale = new Vector3((float)(currHealth)/maxHealth, 1f, 1f);
    }

}
