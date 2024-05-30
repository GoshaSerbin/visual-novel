using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeletePlayerPrefsForDebug : MonoBehaviour
{
    [MenuItem("Utils/Delete All PlayerPrefs")]
    static public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}

