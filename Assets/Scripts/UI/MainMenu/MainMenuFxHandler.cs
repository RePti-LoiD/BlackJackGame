using TMPro;
using UnityEngine;

public class MainMenuFxHandler : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float fadeLerpSpeed;
    [SerializeField] private TMP_Text text;
    private Vector2 targetPosition;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }
    
    public void SetTargetPosition(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void OnEnable()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0, fadeLerpSpeed * Time.deltaTime)); 
    }
}