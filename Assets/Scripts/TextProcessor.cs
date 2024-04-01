using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextProcessor
{
    public static string PostProccess(string aiAnswer)
    {
        var charsToRemove = new string[] { "\n" };
        foreach (var c in charsToRemove)
        {
            aiAnswer = aiAnswer.Replace(c, string.Empty);
        }
        return aiAnswer;
    }

    public static string PreProccess(string prompt)
    {
        return prompt.TrimEnd('\n');
    }
}
