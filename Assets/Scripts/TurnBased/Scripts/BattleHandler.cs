using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleHandler : MonoBehaviour
{
    private enum BattleState
    {
        WAITINGFORPLAYER,
        BUSY
    }

    private static BattleHandler _instance;
    public static BattleHandler GetInstance()
    {
        return _instance;
    }

    [SerializeField] private Transform _enemyBattlePrefab;
    [SerializeField] private Transform _enemyUIPrefab;
    [SerializeField] private Transform _enemyPanel;

    private CharacterBattle _playerCharacter;
    private CharacterBattle _enemyCharacter;
    private CharacterBattle _activeCharacterBattle;

    [SerializeField] private TextMeshProUGUI _characterHealthText;
    [SerializeField] private Image _characterHealthBar;
    [SerializeField] private TextMeshProUGUI _enemyNameText;
    [SerializeField] private TextMeshProUGUI _enemyHealthText;
    [SerializeField] private Image _enemyHealthBar;

    private BattleState _state;

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _enemyCharacter = SpawnEnemy();
        _enemyNameText.text = _enemyCharacter.GetComponent<CharacterBase>()._unitName;
        SetupHealthUI(_enemyCharacter);
        _state = BattleState.WAITINGFORPLAYER;
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBattle>();
        _activeCharacterBattle = _playerCharacter;
        _playerCharacter.Setup();
        SetupHealthUI(_playerCharacter);
    }

    private void Update()
    {
      
    }

    private void SetupHealthUI(CharacterBattle character)
    {
        var currHP = character.GetCurrentHealth();
        var maxHP = character.GetMaxHealth();
        if (character == _playerCharacter)
        {
            _characterHealthText.text = currHP.ToString() + "/" + maxHP.ToString();
            _characterHealthBar.transform.localScale = new Vector3((float)currHP / maxHP, _characterHealthBar.transform.localScale.y, _characterHealthBar.transform.localScale.z);
        }
        else
        {
            _enemyHealthText.text = currHP.ToString() + "/" + maxHP.ToString(); ;
            _enemyHealthBar.transform.localScale = new Vector3((float)currHP / maxHP, _enemyHealthBar.transform.localScale.y, _enemyHealthBar.transform.localScale.z);
        }
    }

    public void CharacterAttack()
    {
        if (_state == BattleState.WAITINGFORPLAYER)
        {
           _state = BattleState.BUSY;
           _playerCharacter.Attack(_enemyCharacter, () => { SetupHealthUI(_enemyCharacter); ChooseNextActiveCharacter(); });
        }

    }

    public void CharacterGuard()
    {
        if (_state == BattleState.WAITINGFORPLAYER)
        {
            _state = BattleState.BUSY;
            _playerCharacter.Guard(() => { ChooseNextActiveCharacter(); });
        }

    }

    private CharacterBattle SpawnEnemy()
    {
        Vector3 position = new Vector3(0, 2f, 0);
        Transform enemyTransform = Instantiate(_enemyBattlePrefab, position, Quaternion.identity);
        GameObject enemyUI = Instantiate(_enemyUIPrefab, _enemyPanel).gameObject;
        Debug.Log("EnemyUI spawned" + enemyUI);
        _enemyNameText = enemyUI.GetComponentInChildren<TextMeshProUGUI>();
        _enemyHealthBar = enemyUI.GetComponentInChildren<Image>();
        _enemyHealthText = enemyUI.GetComponentsInChildren<TextMeshProUGUI>()[1];
        CharacterBattle enemyBattle = enemyTransform.GetComponent<CharacterBattle>();
        enemyBattle.Setup();
        return enemyBattle;
    }

    private void SetActiveCharacterBattle(CharacterBattle character)
    {
        _activeCharacterBattle = character;
    }

    void EnemyAttackWithDelay(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        _enemyCharacter.Attack(_playerCharacter, () => { ChooseNextActiveCharacter(); });
        SetupHealthUI(_playerCharacter);
    }

    private void ChooseNextActiveCharacter()
    {
        if (_activeCharacterBattle == _playerCharacter)
        {
            SetActiveCharacterBattle(_enemyCharacter);
            _state = BattleState.BUSY;
            EnemyAttackWithDelay(1.5f);
        }
        else
        {
            SetActiveCharacterBattle(_playerCharacter);
            _state = BattleState.WAITINGFORPLAYER;
        }
    }
}
