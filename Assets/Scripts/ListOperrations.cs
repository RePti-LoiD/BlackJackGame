using System;
using System.Collections.Generic;

public static class CardListExtension
{
    [Obsolete]
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rand = new System.Random();
        list.Swap(list.Count - 1, rand.Next(0, list.Count - 1));

        for (int i = 1; i < list.Count - 1; i++)
            list.Swap(i, rand.Next(i, list.Count - 1));
    }

    public static List<T> ShuffleList<T>(this IList<T> list)
    {
        Random random = new Random();

        List<T> referenceList = new List<T>(list);
        List<T> shuffledList = new List<T>();
        
        while (referenceList.Count > 0)
        {
            int randomIndex = random.Next(0, referenceList.Count - 1);

            shuffledList.Add(referenceList[randomIndex]);
            referenceList.RemoveAt(randomIndex);
        }

        return shuffledList; 
    }

    public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
    {
        (list[secondIndex], list[firstIndex]) = (list[firstIndex], list[secondIndex]);
    }
}