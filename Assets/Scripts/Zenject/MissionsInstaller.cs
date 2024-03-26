using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MissionsInstaller : MonoInstaller
{

    public GameObject storyPanel;
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI titleText;
    public GameObject choicePanel;
    public GameObject nonMissionPanel;
    public TMP_InputField inputField;
    public Button startButton;

    public override void InstallBindings()
    {
        BindMissionsInstaller();
    }

    private void BindMissionsInstaller()
    {
        Container.Bind<MissionsInstaller>().FromInstance(this).AsSingle();
    }

}
