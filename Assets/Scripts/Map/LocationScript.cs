using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] string _sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
