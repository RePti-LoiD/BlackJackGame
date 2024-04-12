using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuSceneTransition : MonoBehaviour
{
    [SerializeField] private Button offlineBlackJackButton;
    [SerializeField] private Button lanBlackJackButton;
    [SerializeField] private Button legacyBlackJackButton;

    private void Start()
    {
        offlineBlackJackButton.onClick.AddListener(() =>
        {
            BJGameLoader.Data = new BJGameLoadData(null, 
                UserDataWrapper.UserData, 
                new User("Pussy sultan", "[Bot] Valera", "", DateTime.Now.Ticks, null),
                new BJLocalGameManagerFactory());

            SceneManager.LoadScene(6);
        });

        lanBlackJackButton.onClick.AddListener(() =>
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 5;
            data.SceneName = "LAN Blacjack 21";

            SceneManager.LoadScene(1);

        });

        legacyBlackJackButton.onClick.AddListener(() =>
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 3;
            data.SceneName = "Legacy blackjack";

            SceneManager.LoadScene(1);

        });
    }
}