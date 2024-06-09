using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using ModestTree;
using UnityEngine.UIElements;
public class BattleHandler : MonoBehaviour
{

    private static BattleHandler _instance;
    public static BattleHandler GetInstance()
    {
        return _instance;
    }

    [SerializeField] private GameObject _enemyPrefabNoData;
    [SerializeField] private Transform _enemyLayout;
    [SerializeField] private UIHandler _UIHandler;
    [SerializeField] private BattleOverHandler _battleOverHandler;
    [SerializeField] private SliderAttackTime _slider;
    private enum BattleState
    {
        PLAYERTURN,
        BUSY,
        BATTLEEND,
    };

    [SerializeField] private List<EnemyData> _enemyDataAll;
    [SerializeField] private MainCharBattle _playerCharacter;
    private List<EnemyBattle> _enemyCharacters;
    private CharacterBattle _activeCharacterBattle;
    private EnemyBattle _playersTarget;
    private int _activeEnemyIndex = 0;

    private BattleState _state;

    [SerializeField] private List<BattleItemSO> _lootObtained;

    void BattleInitialize()
    {
        _lootObtained = new List<BattleItemSO>();
        _enemyCharacters = new List<EnemyBattle>();
        //int enemiesCount = Random.Range(1, 5);
        int enemiesCount = 4;
        for (int i = 0; i < enemiesCount; ++i)
        {
            int randomIndex = Random.Range(0, _enemyDataAll.Count);
            var enemyGameObj = Instantiate(_enemyPrefabNoData, _enemyLayout);
            var enemyBattleComp = enemyGameObj.GetComponent<EnemyBattle>();
            enemyBattleComp.Setup(_enemyDataAll[randomIndex]);
            enemyGameObj.GetComponent<SpriteRenderer>().sprite = enemyBattleComp.GetSprite();
            _enemyCharacters.Add(enemyBattleComp);
            if (!enemyBattleComp.ThisEnemyLoot.IsEmpty())
                _lootObtained.AddRange(enemyBattleComp.ThisEnemyLoot);
        }
        _playerCharacter.Setup();
    }

    void Awake()
    {
        _instance = this;
        BattleInitialize();
        _UIHandler.SpawnUIs(_enemyCharacters, _playerCharacter);
    }

    private void Start()
    {
        _state = BattleState.PLAYERTURN;
        _activeCharacterBattle = _playerCharacter;
        PlayerTargetSelected(_enemyCharacters[0]);
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.PlayMusic("tense");
        }
        else
        {
            Debug.LogWarning("NO audio manager");
        }
    }

    public void CharacterAttack()
    {
        if (_state == BattleState.PLAYERTURN)
        {
            _state = BattleState.BUSY;
            var lastHealth = _playersTarget.GetCurrentHealth();
            _playerCharacter.Attack(_playersTarget, CalculateDiff(_slider.SliderValue(), _slider.TargetValue()), () => { if (_playersTarget) { _UIHandler.UpdateHealth(_playersTarget, lastHealth); } ChooseNextActiveCharacter(); });
        }

    }

    float CalculateDiff(float sliderVal, float targetVal)
    {
        var diff = Mathf.Abs(sliderVal - targetVal);
        if (diff > 0.2)
        {
            return 0.1f;
        }
        if (diff > 0.1f)
        {
            return 1f;
        }
        if (diff > 0.05f)
        {
            return 1.5f;
        }
        if (diff > 0.04f)
        {
            return 2f;
        }
        return 3f;
    }
    private void ChooseNextActiveCharacter()
    {
        if (_enemyCharacters.Count == 0)
        {
            BattleEnd();
            return;
        }
        if (_activeCharacterBattle == _playerCharacter)
        {
            _activeEnemyIndex = (_activeEnemyIndex + 1) % _enemyCharacters.Count;
            SetActiveCharacterBattle(_enemyCharacters[_activeEnemyIndex]);
            //_activeCharacterBattle.Attack(_playerCharacter, () => { _UIHandler.UpdateHealth(_playerCharacter); ChooseNextActiveCharacter(); });
            EnemyAttackWithDelay(1f);

        }
        else
        {
            SetActiveCharacterBattle(_playerCharacter);
            _slider.PlayerTurnStart();
            _state = BattleState.PLAYERTURN;
        }
    }
    private void SetActiveCharacterBattle(CharacterBattle character)
    {
        _activeCharacterBattle = character;
    }

    public void PlayerTargetSelected(EnemyBattle enemy)
    {
        if (_playersTarget == enemy)
        {
            return;
        }
        if (_playersTarget)
        {
            _playersTarget.DeactivateCircle();
        }
        _playersTarget = enemy;
        _playersTarget.ActivateCircle();
    }

    public EnemyBattle GetPlayerTarget()
    {
        return _playersTarget;
    }

    public void RemoveEnemy(EnemyBattle enemy)
    {
        _UIHandler.UpdateHealth(enemy, 0);
        enemy.gameObject.SetActive(false);
        _enemyCharacters.Remove(enemy);
        if (_enemyCharacters.Count > 0)
        {
            PlayerTargetSelected(_enemyCharacters[0]);
        }
    }

    public void BattleEnd()
    {
        //Debug.Log(_lootObtained);
        Debug.Log("YOU WIN");
        _battleOverHandler.ChangeToBattleEnd(_lootObtained);
    }

    public void PlayerLost()
    {
        _UIHandler.UpdateHealth(_playerCharacter, 0);
        Debug.Log("YOU LOST");
        _battleOverHandler.ChangeToBattleEnd(new List<BattleItemSO>());
    }

    void EnemyAttackWithDelay(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        var lastHealth = _playerCharacter.GetCurrentHealth();
        _activeCharacterBattle.Attack(_playerCharacter, 0, () => { if (_playerCharacter) _UIHandler.UpdateHealth(_playerCharacter, lastHealth); ChooseNextActiveCharacter(); });
    }
}

//    public void CharacterGuard()
//    {
//        if (_state == BattleState.WAITINGFORPLAYER)
//        {
//            _state = BattleState.BUSY;
//            _playerCharacter.Guard(() => { ChooseNextActiveCharacter(); });
//        }

//    }

