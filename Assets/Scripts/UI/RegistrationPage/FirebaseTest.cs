using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    }
}