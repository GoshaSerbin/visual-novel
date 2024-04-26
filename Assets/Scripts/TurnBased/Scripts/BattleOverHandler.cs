using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleOverHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private int _maxSkillPoints = 0;
    private int _skillPointsUsed = 0;
    [SerializeField] TextMeshProUGUI _spText;
    [SerializeField] TextMeshProUGUI _spUsedText;

    [SerializeField] GameObject _battleCanvas;
    [SerializeField] GameObject _postBattleCanvas;
    void Start()
    {
        
    }

    public void ChangeToBattleEnd(int skillPoints)
    {
        _maxSkillPoints = skillPoints;
        _battleCanvas.SetActive(false);
        _postBattleCanvas.SetActive(true);
        _spText.text = "Доступные очки: " + _maxSkillPoints.ToString();
    }
    public void AddToUsedPoints()
    {
        if (_skillPointsUsed < _maxSkillPoints)
        {
            _skillPointsUsed++;
            _spUsedText.text = "Очков использовано: " + _skillPointsUsed.ToString();
        }
    }

    public void SubtractFromUsedPoints()
    {
        if (_skillPointsUsed >= 1)
        {
            _skillPointsUsed--;
            _spUsedText.text = "Очков использовано: " + _skillPointsUsed.ToString();
        }
    }
}
