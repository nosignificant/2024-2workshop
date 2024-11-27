using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour
{

    public static Dictionary<int, (string character, int[] sequence)> consonant = new Dictionary<int, (string, int[])>
    {
        { 1, ("ㄱ", new int[] { 1 , 0}) },
        { 2, ("ㄴ", new int[] { 4, 0 }) },
        { 3, ("ㄷ", new int[] { 2,0 }) },
        { 4, ("ㄹ", new int[] { 2,5,0 }) },
        { 5, ("ㅁ", new int[] { 3,0 }) },
        { 6, ("ㅂ", new int[] { 2,8,0 }) },
        { 7, ("ㅅ", new int[] { 7,0 }) },
        { 8, ("ㅇ", new int[] { 0 }) },
        { 9, ("ㅈ", new int[] { 7,8 , 0 }) },
        { 10, ("ㅊ", new int[] { 8,7,8,0 }) },
        { 11, ("ㅋ", new int[] { 1,8,0 }) },
        { 12, ("ㅌ", new int[] { 8,2,0 }) },
        { 13, ("ㅍ", new int[] { 8,8,8,0 }) },
        { 14, ("ㅎ", new int[] { 8,8,0 }) },

    };

    public static Dictionary<int, (string character, int[] sequence)> vowel = new Dictionary<int, (string, int[])>
    { // 좌 우 상 하  
        { 1, ("ㅏ", new int[] { 1 }) },
        { 2, ("ㅑ", new int[] { 4 }) },
        { 3, ("ㅐ", new int[] { 2 }) },
        { 4, ("ㅒ", new int[] { 2,5 }) },
        { 5, ("ㅓ", new int[] { 3 }) },
        { 6, ("ㅕ", new int[] { 2,8 }) },
        { 7, ("ㅔ", new int[] { 7 }) },
        { 8, ("ㅖ", new int[] {  }) },
        { 9, ("ㅗ", new int[] { 7,8 }) },
        { 10, ("", new int[] { 8,7,8 }) },
        { 11, ("ㅜ", new int[] { 1,8 }) },
        { 12, ("ㅠ", new int[] { 8,2 }) },
        { 13, ("ㅡ", new int[] { 8,8,8 }) },
        { 14, ("ㅣ", new int[] { 8,8 }) },

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

