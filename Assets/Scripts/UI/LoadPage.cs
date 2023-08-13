using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI continueText;

    [SerializeField] private TextMeshProUGUI loadPercent;
    [SerializeField] private TextMeshProUGUI sceneNameText;

    [SerializeField] private Image loadImage;

    private NextSceneData nextSceneData;

    private void Start()
    {
        continueText.gameObject.SetActive(false);

        nextSceneData = NextSceneData.Init();

        sceneNameText.text = nextSceneData.SceneName;

        StartCoroutine(LoadSceneAsync(nextSceneData.SceneIndex));
    }

    private void Update()
    {
        if (continueText.gameObject.activeSelf)
            continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, Mathf.PingPong(Time.time, 1));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        var loadData = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        loadData.allowSceneActivation = false;
        
        while (!loadData.isDone)
        {
            loadPercent.text = $"{(int)Mathf.Round((loadData.progress / 0.009f))}%";
            loadImage.fillAmount = loadData.progress / 0.9f;
            
            if (loadData.progress == 0.9f)
            {
                continueText.gameObject.SetActive(true);

                if (Input.anyKey)
                {
                    loadData.allowSceneActivation = true;

                    yield break;
                }
            }
            
            yield return null;
        }
    }
}