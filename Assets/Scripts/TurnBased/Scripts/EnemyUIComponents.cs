using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIComponents : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Transform _healthBar;

    public TextMeshProUGUI NameText { get { return _nameText; } set { _nameText = value; } }
    public TextMeshProUGUI HealthText { get { return _healthText; } set { _healthText = value; } }
    public Transform HealthBar { get { return _healthBar; } set { _healthBar = value; } }
}
