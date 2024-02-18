using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private TextMeshProUGUI currentDate;

    private void Start()
    {
        Application.targetFrameRate = 60;

        continueText.text = "Hello, " + UserDataWrapper.UserData?.NickName ?? "guest";
    }

    private void Update()
    {
        currentDate.transform.localScale = new Vector3(Mathf.Clamp(Mathf.PingPong(Time.time * 2, 1), 0.7f, 1.2f), 
                                                          Mathf.Clamp(Mathf.PingPong(Time.time * 2, 1), 0.7f, 1.2f), 1);

        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, Mathf.PingPong(Time.time, 1));

        bool isTouch = false;

#if UNITY_EDITOR
        isTouch = Input.anyKeyDown;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        isTouch = Input.GetTouch(0).phase == TouchPhase.Ended;
#endif

#if UNITY_STANDALONE
        isTouch = Input.anyKey;
#endif

        if (isTouch)
        {
            NextSceneData data = NextSceneData.Init();
            
            if (UserDataWrapper.UserData != null)
                SceneManager.LoadScene(2);
            else
                SceneManager.LoadScene(4);
        }
    }
}