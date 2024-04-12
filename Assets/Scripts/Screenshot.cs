using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "Logo.png", 1);
    }
}