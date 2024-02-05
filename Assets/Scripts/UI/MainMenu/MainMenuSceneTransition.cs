using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSceneTransition : MonoBehaviour
{
    [SerializeField] private Button offlineBlackJackButton;
    [SerializeField] private Button lanBlackJackButton;

    private void Start()
    {
        offlineBlackJackButton.onClick.AddListener(() =>
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 3;
            data.SceneName = "Blacjack 21";

            SceneManager.LoadScene(1);
        });

        lanBlackJackButton.onClick.AddListener(() =>
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 5;
            data.SceneName = "LAN Blacjack 21";

            SceneManager.LoadScene(1);

        });
    }
}