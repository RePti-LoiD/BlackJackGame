using UnityEngine;

public class ChatMessageHandler : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform chatTransform;

    public void HandleMessage(User user, string message)
    {
        GameObject instantObj = Instantiate(messagePrefab, chatTransform);
        instantObj.GetComponent<ChatMessage>().VisualizeMessage(user, message);
    }
}
