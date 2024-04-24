using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LocationScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string _sceneName;
    [SerializeField] string[] _inventoryOnLoc;
    [SerializeField] string[] _peopleOnLoc;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
