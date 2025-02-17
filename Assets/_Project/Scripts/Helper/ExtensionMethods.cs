using System.Collections.Generic;
using System;
using UnityEngine;

namespace _Project.Scripts.Helper
{
    public static class ExtensionMethods
    {
       public static void Shuffle<T>(this List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
        public static IEnumerable<Transform> GetChilds(this Transform parent, bool includeInactive = false)
        {
            if (parent == null) yield break;

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                if (!includeInactive && !child.gameObject.activeInHierarchy)
                    continue;

                yield return child;
                 
                foreach (var grandChild in child.GetChilds(includeInactive))
                {
                    yield return grandChild;
                }
            }
        }
    }
}