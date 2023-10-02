using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using Firebase;

public class FirebaseTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private Button continueAsGuest;
    [SerializeField] private bool isGet;

    [Header("Reg")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField nickNameInputField;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField repeatPasswordInputField;
    [SerializeField] private Button createUserButton;

    [Header("SignIn")]
    [SerializeField] private TMP_InputField signInEmailInputField;
    [SerializeField] private TMP_InputField signInPasswordInputField;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signOutButton;

    private DatabaseReference databaseReference;
    private FirebaseApp firebaseApp;
    private FirebaseAuth auth;

    private bool isCreate;

    private void Start()
    {
        continueAsGuest.onClick.AddListener(() =>
        {
            print(IsGuest.Guest.ToString());
            PlayerPrefs.SetString(PlayerPrefsKeys.IsGuest, IsGuest.Guest.ToString());

            NextSceneData nextSceneData = NextSceneData.Init();
            nextSceneData.SceneIndex = 2;
            nextSceneData.SceneName = "MainMenu";

            SceneManager.LoadScene(1);
        });

        try
        {
            debugText.text += $"Fixing dependencies";
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            auth = FirebaseAuth.DefaultInstance;

            debugText.text += $"FAuth {FirebaseAuth.DefaultInstance}\nDB {FirebaseDatabase.DefaultInstance.RootReference}";

            signOutButton.onClick.AddListener(() => StartCoroutine(SignOut()));

            auth.StateChanged += (sender, args) =>
            {
                if (auth.CurrentUser != null && isCreate)
                {
                    databaseReference
                        .Child("users")
                        .Child(auth.CurrentUser.UserId)
                        .SetRawJsonValueAsync(
                            JsonConvert.SerializeObject(new User(nameInputField.text, nickNameInputField.text, emailInputField.text, 1000, string.Empty,
                                        ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds())));

                    string message = $"<color=#7d7d7d>Пользователь <size=1.5em><color=green>вошёл в сеть</color></size>!</color><br>" +
                        $"<color=#7d7d7d>ID пользователя <color=yellow>{auth.CurrentUser.UserId}</color></color>";
                    infoText.text = message;

                    isCreate = false;

                    Invoke(nameof(NavigateToMainMenu), 2f);
                }
                else if (auth.CurrentUser != null)
                {
                    infoText.text = $"<color=#7d7d7d>User id <color=yellow>{auth?.CurrentUser?.UserId}</color></color>";

                    Invoke(nameof(NavigateToMainMenu), 2f);
                }
            };
        }
        catch (Exception e)
        {
            debugText.text += e.Message + "\n" + e.StackTrace + "\n" + e.Source;
        }
    }

    public void Create()
    {
        if (passwordInputField.text != repeatPasswordInputField.text)
        {
            infoText.text = $"<color=#7d7d7d>Пароли <size=1.5em><color=yellow>не совпадают</color></size>.</color>";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text);

        isCreate = true;
    }

    public void SignIn()
    {
        debugText.text += $"{auth} | sign In pressed\n";

        auth.SignInWithEmailAndPasswordAsync(signInEmailInputField.text, signInPasswordInputField.text);

        debugText.text += $"after call (SignInWithEmailAndPasswordAsync)";
    }

    public IEnumerator SignOut()
    {
        Task<DataSnapshot> data = databaseReference.Child("users").Child(auth.CurrentUser.UserId).GetValueAsync();

        yield return new WaitUntil(() => data.IsCompleted);
        
        infoText.text = $"{JObject.Parse(data.Result.GetRawJsonValue())["NickName"]}Signed <color=yellow>out</color>";
        
        auth.SignOut();
    }

    private void NavigateToMainMenu()
    {
        NextSceneData nextSceneData = NextSceneData.Init();

        nextSceneData.SceneIndex = 2;
        nextSceneData.SceneName = "MainMenu";

        SceneManager.LoadScene(1);
    }
}