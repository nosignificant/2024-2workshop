using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour
{

    public static Dictionary<int, (string character, int[] sequence)> consonant = new Dictionary<int, (string, int[])>
    {
        { 1, ("ㄱ", new int[] { 1 }) },       
        { 2, ("ㄴ", new int[] { 4 }) },       
        { 3, ("ㄷ", new int[] { 2 }) },
        { 4, ("ㄹ", new int[] { 2,5 }) },
        { 5, ("ㅁ", new int[] { 3 }) },
        { 6, ("ㅂ", new int[] { 2, }) },
        { 7, ("ㅅ", new int[] { 3 }) },
        { 8, ("ㅇ", new int[] { 3 }) },
        { 9, ("ㅈ", new int[] { 3 }) },
        { 10, ("ㅊ", new int[] { 3 }) },
        { 11, ("ㅋ", new int[] { 3 }) },
        { 12, ("ㅌ", new int[] { 3 }) },
        { 13, ("ㅍ", new int[] { 3 }) },
        { 14, ("ㅎ", new int[] { 3 }) },

    };

    public static string FindConsonant(List<int> signs)
    {
        foreach (var kvp in consonant)
        {
            // 동일한 배열인지 확인
            if (AreListsEqual(signs, new List<int>(kvp.Value.sequence)))
            {
                Debug.Log($"Matched Key: {kvp.Key}, Character: {kvp.Value.character}");
                return kvp.Value.character; // 자음 반환
            }
        }

        Debug.LogWarning("No matching consonant found!");
        return null; // 일치하는 자음이 없을 경우
    }
    private static bool AreListsEqual(List<int> list1, List<int> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }
}

