using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerProfilePicture : MonoBehaviour
{

    private Image _playerProfilePicture;
    [SerializeField] private Sprite _fallbackPicture;

    void Awake()
    {
        _playerProfilePicture = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePicture();
    }

    public void UpdatePicture()
    {
        _playerProfilePicture.sprite = AIManager.LoadSpriteFromPNG("PlayerProfilePicture");
        if (_playerProfilePicture.sprite == null)
        {
            _playerProfilePicture.sprite = _fallbackPicture;
        }
    }

}
