using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickName;
    [SerializeField] private TMP_InputField firstName;
    [SerializeField] private Button button;
    [SerializeField] private User userData;

    [SerializeField] private TMP_Text text;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            try
            {
                UserDataWrapper.UserData = new User(firstName.text, nickName.text, null, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), new Wallet(100));

                SceneManager.LoadScene(2);
            }
            catch (Exception e)
            {
                text.text = e.Message + "\b" + e.StackTrace;
            }
        });
    }
}