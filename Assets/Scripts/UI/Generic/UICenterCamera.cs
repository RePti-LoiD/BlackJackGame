using UnityEngine;

public class UICenterCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;

    private void Start()
    {
        cam.transform.position = new Vector3(canvas.pixelRect.width / 2, canvas.pixelRect.height / 2, -999);
    }
}