using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesInstaller : MonoBehaviour
{
    public TextAsset inkJson;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Button nextPhraseButton;

    public GameObject choiceButtonsPanel;
    public GameObject choiceButton;

    public GameObject inputFieldPanel;
    public TMP_InputField inputField;
    public Button stopTalkButton;
    public Button continueTalkButton;
}
