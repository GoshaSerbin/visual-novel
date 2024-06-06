using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public void ToMainMenuFun()
    {
        var lvlLoader = FindObjectOfType<LvlLoader>();
        if (lvlLoader != null)
        {
            lvlLoader.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

}
