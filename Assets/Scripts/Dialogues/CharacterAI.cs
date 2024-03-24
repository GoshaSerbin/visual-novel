using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{

    [TextArea(3, 10)][SerializeField] private string _characterDescription = "";
    [SerializeField] private int _contextMemory = 1;
    int iter = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    public string Ask(string phrase)
    {
        iter += 1;
        return "пошел в жопу " + iter;
    }
}
