using UnityEngine;

[CreateAssetMenu(menuName = "PhrasesContainer/UserDataPhrase")]
public class UserDataPhrase : Phrase
{
    [Header("use {UserData.Propery} syntax to set inline data")]
    [SerializeField] private User user;

    public override string GetPhrase()
    {
        return "";
    }
}