using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI continueText;

    private void Update()
    {
        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, Mathf.PingPong(Time.time, 1));
        
        if (Input.anyKey || Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            NextSceneData data = NextSceneData.Init();
            data.SceneIndex = 2;
            data.SceneName = "MainMenu";

            SceneManager.LoadScene(1);
        }
    }
}