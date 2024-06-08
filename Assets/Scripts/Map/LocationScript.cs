using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LocationScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string _sceneName;
    [SerializeField] string _inventoryOnLoc;
    [SerializeField] string _peopleOnLoc;

    [SerializeField] TextMeshProUGUI _peopleText;
    [SerializeField] TextMeshProUGUI _inventoryText;

    private void OnMouseDown()
    {
        FindObjectOfType<LvlLoader>()?.LoadScene(_sceneName);
    }

    private void OnMouseEnter()
    {
        _peopleText.text = _peopleOnLoc;
        _inventoryText.text = _inventoryOnLoc;
    }
    private void OnMouseExit()
    {
        _peopleText.text = "";
        _inventoryText.text = "";
    }
}
