using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Runtime.CompilerServices;
public class UIHandler : MonoBehaviour
{

    [SerializeField] private GameObject _enemyUIPrefab;
    [SerializeField] private Transform _parentPanel;

    [SerializeField] private TextMeshProUGUI _mainCharHealthText;
    [SerializeField] private Transform _mainCharHealthBar;
    private MainCharBattle _mainCharBattle;

    [SerializeField] private Dictionary<EnemyBattle, EnemyUIComponents> _enemyUIs = new();

    private float _smoothDecreaseDuration = 0.5f;

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
        float damage = maxHealth - currHealth;
        if (character == _mainCharBattle)
        {
            _mainCharHealthText.text = currHealth.ToString() + "/" + maxHealth.ToString();
            StartCoroutine(SmoothDecreaseHealth(damage, currHealth, maxHealth));
            return;
        }
        EnemyBattle enemy = character as EnemyBattle;
        _enemyUIs[enemy].HealthText.text = currHealth.ToString() + "/" + maxHealth.ToString();
        //StartCoroutine(SmoothDecreaseHealthEnemy(enemy, damage, currHealth));
        _enemyUIs[enemy].HealthBar.localScale = new Vector3((float)currHealth / maxHealth, 1f, 1f);
    }


    private IEnumerator SmoothDecreaseHealth(float damage, float currHealth, int cHealth)
    {
        float damagePerTick = damage / _smoothDecreaseDuration;
        float elapsedTime = 0f;
        float currentHealth = currHealth;

        while (elapsedTime < _smoothDecreaseDuration)
        {
            Debug.Log(_mainCharHealthBar.localScale.x);
            float currentDamage = damagePerTick * Time.deltaTime;  
            currentHealth -= currentDamage;
            elapsedTime += Time.deltaTime;
            Debug.Log(currentHealth / cHealth);
            _mainCharHealthBar.localScale = new Vector3(currentHealth / cHealth, 1f, 1f);
            if (currentHealth <= 0)
            {
                _mainCharHealthBar.localScale = new Vector3(0f, 1f, 1f);
            }
            yield return null;
        }
        yield return null;
    }

    private IEnumerator SmoothDecreaseHealthEnemy(EnemyBattle enemy, float damage, int cHealth)
    {
        float damagePerTick = damage / _smoothDecreaseDuration;
        float elapsedTime = 0f;
        float currentHealth = cHealth;

        while (elapsedTime < _smoothDecreaseDuration)
        {
            float currentDamage = damagePerTick * Time.deltaTime;
            currentHealth -= currentDamage;
            elapsedTime += Time.deltaTime;
            _enemyUIs[enemy].HealthBar.localScale = new Vector3(currentHealth / cHealth, 1f, 1f);
            if (currentHealth <= 0) {
                _enemyUIs[enemy].HealthBar.localScale = new Vector3(0f, 1f, 1f);
            }
            yield return null;
        }
        yield return null;
    }
}
