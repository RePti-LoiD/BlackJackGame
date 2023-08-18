using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSceneTransition : MonoBehaviour
{
    [SerializeField] private Button changeSceneButton;

    private void Start()
    {
        changeSceneButton.onClick.AddListener(() =>
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 3;
            data.SceneName = "Blacjack 21";

            SceneManager.LoadScene(1);
        });
    }
}