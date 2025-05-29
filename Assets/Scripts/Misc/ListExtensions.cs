using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            int r = Random.Range(i, n);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
