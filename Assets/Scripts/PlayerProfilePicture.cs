using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerProfilePicture : MonoBehaviour
{

    private Image _playerProfilePicture;
    [SerializeField] private Sprite _fallbackPicture;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    void Awake()
    {
        _playerProfilePicture = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePicture();
    }

    public void UpdatePicture() // and name
    {
        _playerProfilePicture.sprite = AIManager.LoadSpriteFromPNG("PlayerProfilePicture");
        _playerNameText.text = PlayerPrefs.GetString("PlayerName", "Игрок");
        if (_playerProfilePicture.sprite == null)
        {
            _playerProfilePicture.sprite = _fallbackPicture;
        }
    }

}
