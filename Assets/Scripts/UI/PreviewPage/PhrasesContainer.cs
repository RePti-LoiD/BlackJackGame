using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PhrasesContainer/Container")]
public class PhrasesContainer : ScriptableObject
{
    [SerializeField] private List<Phrase> phrases;

    public string GetRandomPhrase()
    {
        System.Random random = new System.Random();
        int randIndex = random.Next(phrases.Count);

        return phrases[randIndex].GetPhrase();
    }
}
