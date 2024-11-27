using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour
{

    public static Dictionary<int, (string character, int[] sequence)> consonant = new Dictionary<int, (string, int[])>
    {
        { 1, ("��", new int[] { 1 , 0}) },
        { 2, ("��", new int[] { 4, 0 }) },
        { 3, ("��", new int[] { 2,0 }) },
        { 4, ("��", new int[] { 2,5,0 }) },
        { 5, ("��", new int[] { 3,0 }) },
        { 6, ("��", new int[] { 2,8,0 }) },
        { 7, ("��", new int[] { 7,0 }) },
        { 8, ("��", new int[] { 0 }) },
        { 9, ("��", new int[] { 7,8 , 0 }) },
        { 10, ("��", new int[] { 8,7,8,0 }) },
        { 11, ("��", new int[] { 1,8,0 }) },
        { 12, ("��", new int[] { 8,2,0 }) },
        { 13, ("��", new int[] { 8,8,8,0 }) },
        { 14, ("��", new int[] { 8,8,0 }) },

    };

    public static Dictionary<int, (string character, int[] sequence)> vowel = new Dictionary<int, (string, int[])>
    { // �� �� �� ��  
        { 1, ("��", new int[] { 1 }) },
        { 2, ("��", new int[] { 4 }) },
        { 3, ("��", new int[] { 2 }) },
        { 4, ("��", new int[] { 2,5 }) },
        { 5, ("��", new int[] { 3 }) },
        { 6, ("��", new int[] { 2,8 }) },
        { 7, ("��", new int[] { 7 }) },
        { 8, ("��", new int[] {  }) },
        { 9, ("��", new int[] { 7,8 }) },
        { 10, ("", new int[] { 8,7,8 }) },
        { 11, ("��", new int[] { 1,8 }) },
        { 12, ("��", new int[] { 8,2 }) },
        { 13, ("��", new int[] { 8,8,8 }) },
        { 14, ("��", new int[] { 8,8 }) },

    };

    public static string FindConsonant(List<int> signs)
    {
        foreach (var kvp in consonant)
        {
            // ������ �迭���� Ȯ��
            if (AreListsEqual(signs, new List<int>(kvp.Value.sequence)))
            {
                Debug.Log($"Matched Key: {kvp.Key}, Character: {kvp.Value.character}");
                return kvp.Value.character; // ���� ��ȯ
            }
        }

        Debug.LogWarning("No matching consonant found!");
        return null; // ��ġ�ϴ� ������ ���� ���
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

