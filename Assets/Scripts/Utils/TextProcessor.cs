using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextProcessor
{

    public static int MaxLength = 300;
    public static string PostProccess(string aiAnswer)
    {
        // var charsToRemove = new string[] { "\"" };
        // foreach (var c in charsToRemove)
        // {
        //     aiAnswer = aiAnswer.Replace(c, string.Empty);
        // }
        aiAnswer = aiAnswer.Trim('\"');
        aiAnswer = aiAnswer.Replace('\n', ' ');
        while (aiAnswer.Length > MaxLength)
        {
            Debug.LogWarning("To much content...Will reduce it. FullMessage: " + aiAnswer);
            int index = aiAnswer.LastIndexOf('.');
            if (index >= 0)
            {
                if (index < aiAnswer.Length - 1)
                {
                    aiAnswer = aiAnswer.Substring(0, index + 1);
                }
                else
                {
                    aiAnswer = aiAnswer.Substring(0, index);
                }
            }
            else
            {
                aiAnswer = aiAnswer.Substring(0, MaxLength - 5) + "...";
                break;
            }

        }
        return aiAnswer;
    }

    public static string PreProccess(string prompt)
    {
        return prompt.TrimEnd('\n');
    }

    public static string GetChoice(string aiAnswer, int choice)
    {
        string separator;
        if (aiAnswer.Contains("1.") && aiAnswer.Contains("2."))
        {
            separator = ".";
        }
        else if (aiAnswer.Contains("1)") && aiAnswer.Contains("2)"))
        {
            separator = ")";
        }
        else
        {
            Debug.LogWarning("Все пошло не по плану))))");
            return "Ошибочка....";
        }
        var BeginInd = aiAnswer.IndexOf(choice + separator) + 2;
        var EndInd = aiAnswer.IndexOf(choice + 1 + separator);
        if (EndInd > 0)
        {
            string answer = aiAnswer.Substring(BeginInd, EndInd - BeginInd);
            return answer;
        }
        return aiAnswer.Substring(BeginInd);
    }
}
