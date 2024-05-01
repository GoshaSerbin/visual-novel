using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextEffects : MonoBehaviour
{
    private TMP_Text _textComponent;

    public bool IsAnimatingText { get; private set; }

    [SerializeField] private float _secondsForOneSymbol = 0.02f;

    private void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
        IsAnimatingText = false;
    }


    private void UpdateMesh()
    {
        _textComponent.ForceMeshUpdate();
        var textInfo = _textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                var index = charInfo.vertexIndex + j;
                var orig = verts[index];
                // verts[index] = orig + new Vector3(0, 10 / (10 - (orig.x - verts[0].x) + (verts[1].y - orig.y) + 0.1f * Time.time), 0);
                // подумоть
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    public void Display(string text)
    {
        IsAnimatingText = true;
        StopAllCoroutines(); // is it ok?
        StartCoroutine(TypeSentence(text));
    }

    public void CompleteDisplay()
    {
        IsAnimatingText = false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        _textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _textComponent.text += letter;
            UpdateMesh();
            if (IsAnimatingText)
            {
                yield return new WaitForSeconds(_secondsForOneSymbol);
            }
        }
        IsAnimatingText = false;
    }
}
