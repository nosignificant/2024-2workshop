using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabet : MonoBehaviour
{

    public static Dictionary<int, (string character, int[] sequence)> consonant = new Dictionary<int, (string, int[])>
    {
        { 1, ("��", new int[] { 1 }) },       
        { 2, ("��", new int[] { 4 }) },       
        { 3, ("��", new int[] { 2 }) },
        { 4, ("��", new int[] { 2,5 }) },
        { 5, ("��", new int[] { 3 }) },
        { 6, ("��", new int[] { 2, }) },
        { 7, ("��", new int[] { 3 }) },
        { 8, ("��", new int[] { 3 }) },
        { 9, ("��", new int[] { 3 }) },
        { 10, ("��", new int[] { 3 }) },
        { 11, ("��", new int[] { 3 }) },
        { 12, ("��", new int[] { 3 }) },
        { 13, ("��", new int[] { 3 }) },
        { 14, ("��", new int[] { 3 }) },

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

