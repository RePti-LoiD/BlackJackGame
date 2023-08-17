using TMPro;
using Mirror;
using UnityEngine;

public class SyncText : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] [SyncVar] private string synchronizedText;

    private void Start()
    {
        if (!isOwned)
        {
            inputField.gameObject.SetActive(false);
            return;
        }

        inputField.onEndEdit.AddListener((text) =>
        {
            synchronizedText = text;
        });
    }   
}