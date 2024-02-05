using UnityEngine;

public abstract class Phrase : ScriptableObject
{
    [SerializeField] protected string phraseText;

    public abstract string GetPhrase();
}