using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private void Update()
    {
        fpsText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}