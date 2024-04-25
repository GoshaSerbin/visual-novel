using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    private enum BattleState
    {
        PLAYERTURN,
        BUSY,
        BATTLEEND,
    };

    [SerializeField] private List<EnemyData> _enemyDataAll;
    [SerializeField] private MainCharBattle _playerCharacter;
    [SerializeField] private List<EnemyBattle> _enemyCharacters;
    [SerializeField] private CharacterBattle _activeCharacterBattle;
    private EnemyBattle _playersTarget;
    private int _activeEnemyIndex = 0;

    private BattleState _state;

    void BattleInitialize()
    {
        int enemiesCount = Random.Range(1, 5);
        for (int i = 0; i < enemiesCount; ++i)
        {
            int randomIndex = Random.Range(0, _enemyDataAll.Count);
            var enemyGameObj = Instantiate(_enemyPrefabNoData, _enemyLayout);
            var enemyBattleComp = enemyGameObj.GetComponent<EnemyBattle>();
            enemyBattleComp.Setup(_enemyDataAll[randomIndex]);
            enemyGameObj.GetComponent<SpriteRenderer>().sprite = enemyBattleComp.GetSprite();
            _enemyCharacters.Add(enemyBattleComp);
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
    }

    public void CharacterAttack()
    {
        if (_state == BattleState.PLAYERTURN)
        {
            _state = BattleState.BUSY;
            Debug.Log(_state);
            _playerCharacter.Attack(_playersTarget, () => { if (_playersTarget) { _UIHandler.UpdateHealth(_playersTarget); } ChooseNextActiveCharacter(); });
        }

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
            _activeCharacterBattle.Attack(_playerCharacter, () => { _UIHandler.UpdateHealth(_playerCharacter); ChooseNextActiveCharacter(); });

        }
        else
        {
            SetActiveCharacterBattle(_playerCharacter);
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
        _UIHandler.UpdateHealth(enemy);
        enemy.gameObject.SetActive(false);
        _enemyCharacters.Remove(enemy);
        if (_enemyCharacters.Count > 0)
        {
            PlayerTargetSelected(_enemyCharacters[0]);
        }
    }

    public void BattleEnd()
    {
        Debug.Log("YOU WON");
    }

    public void PlayerLost()
    {
        _UIHandler.UpdateHealth(_playerCharacter);
        Debug.Log("YOU LOST");
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


//    void EnemyAttackWithDelay(float delayTime)
//    {
//        StartCoroutine(DelayAction(delayTime));
//    }

//    IEnumerator DelayAction(float delayTime)
//    {
//        yield return new WaitForSeconds(delayTime);

//        _enemyCharacter.Attack(_playerCharacter, () => { ChooseNextActiveCharacter(); });
//        SetupHealthUI(_playerCharacter);
//    }
//}
