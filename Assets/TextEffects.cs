using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextEffects : MonoBehaviour
{
    private TMP_Text _textComponent;

    [SerializeField] private float _secondsForOneSymbol = 0.02f;

    void Start()
    {
        _textComponent = GetComponent<TMP_Text>();
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
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
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
        StopAllCoroutines(); // is it ok?
        StartCoroutine(TypeSentence(text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        _textComponent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _textComponent.text += letter;
            UpdateMesh();
            yield return new WaitForSeconds(_secondsForOneSymbol);
        }
    }
}