using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseTest : MonoBehaviour
{
    [Header("Reg")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField firstNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button createUserButton;

    [Header("SignIn")]
    [SerializeField] private TMP_InputField signInNameInputField;
    [SerializeField] private TMP_InputField signInPasswordInputField;
    [SerializeField] private Button signInButton;

    //[SerializeField] private DatabaseReference databaseReference;

    /*private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        createUserButton.onClick.AddListener(() =>
            databaseReference
            .Child("users")
            .Child(nameInputField.text)
            .SetRawJsonValueAsync(JsonConvert.SerializeObject(new
            {
                name = nameInputField.text,
                fname = firstNameInputField.text,
                password = passwordInputField.text,
            })
        ));

        signInButton.onClick.AddListener(() =>
        {
            StartCoroutine(WaitResponse());
        });
    }

    private IEnumerator WaitResponse()
    {
        System.Threading.Tasks.Task<DataSnapshot> data = databaseReference.Child("users").Child(signInNameInputField.text).GetValueAsync();


        yield return new WaitUntil(() => data.IsCompleted);
        Debug.Log("Data received");

        JToken dataObject = JObject.Parse(data.Result.GetRawJsonValue())["fname"];

        Debug.Log((string)dataObject);
    }*/
}