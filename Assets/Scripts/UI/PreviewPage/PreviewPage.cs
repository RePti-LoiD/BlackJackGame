using Firebase.Auth;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewPage : MonoBehaviour
{
    [SerializeField] private UserDataLoader userDataLoader;

    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private TextMeshProUGUI currentDate;

    private FirebaseAuth firebaseAuth;

    private void Start()
    {
        Application.targetFrameRate = 60;
        currentDate.text = DateTime.Now.ToString("d MMMM");

        firebaseAuth = FirebaseAuth.DefaultInstance;

        userDataLoader.OnDataLoad += (user) => 
        {
            if (user != null)
                continueText.text = "Hello, " + user.NickName;
        };
    }

    private void Update()
    {
        currentDate.transform.localScale = new Vector3(Mathf.Clamp(Mathf.PingPong(Time.time * 2, 1), 0.7f, 1.2f), 
                                                          Mathf.Clamp(Mathf.PingPong(Time.time * 2, 1), 0.7f, 1.2f), 1);

        continueText.color = new Color(continueText.color.r, continueText.color.g, continueText.color.b, Mathf.PingPong(Time.time, 1));

        bool isTouch = false;

#if UNITY_EDITOR
        isTouch = Input.anyKeyDown;
        print("editor");
#endif

#if UNITY_ANDROID && !UNITY_EDITOR 
        isTouch = Input.GetTouch(0).phase == TouchPhase.Ended;
#endif

        if (isTouch)
        {
            NextSceneData data = NextSceneData.Init();
            
            if (firebaseAuth?.CurrentUser != null || PlayerPrefs.HasKey(PlayerPrefsKeys.IsGuest))
            {
                data.SceneIndex = 2;
                data.SceneName = "MainMenu";

                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(4);
            }
        }
    }
}