using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextProcessor
{

    public static int MaxLength = 400;
    public static string PostProccess(string aiAnswer)
    {
        var charsToRemove = new string[] { "\n", "\"" };
        foreach (var c in charsToRemove)
        {
            aiAnswer = aiAnswer.Replace(c, string.Empty);
        }
        aiAnswer = aiAnswer.Replace('\n', ' ');
        while (aiAnswer.Length > 400)
        {
            Debug.LogWarning("To much content...Will reduce it");
            int index = aiAnswer.LastIndexOf('.');
            if (index >= 0)
            {
                aiAnswer = aiAnswer.Substring(0, index + 1);
            }
            else
            {
                aiAnswer = aiAnswer.Substring(0, MaxLength) + "...";
                break;
            }
        }
        return aiAnswer;
    }

    public static string PreProccess(string prompt)
    {
        return prompt.TrimEnd('\n');
    }
}
