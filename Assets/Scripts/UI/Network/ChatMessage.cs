using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI senderNameText;
    [SerializeField] private RectTransform messageCover;

    private User sender;

    public void VisualizeMessage(User sender, string message)
    {
        this.sender = sender;

        senderNameText.text = sender.NickName;
        messageText.text = message;

        messageCover.rect.Set(messageCover.rect.x, messageCover.rect.y, messageCover.rect.width, messageCover.rect.height + messageText.renderedHeight);
    }
}
