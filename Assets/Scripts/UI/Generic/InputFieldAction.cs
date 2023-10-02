using TMPro;
using UnityEngine;

public class InputFieldAction : MonoBehaviour
{
    [SerializeField] private UiChangeAction action;

    private TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();

        inputField.onSelect.AddListener((text) =>
        {
            action.EnabledAction();
        });
        inputField.onDeselect.AddListener((text) =>
        {
            action.DisabledAction();
        });
    }
}