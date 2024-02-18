using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private ModalFrame modalFrame;
    [SerializeField] private int sceneIndex;

    [Header("Modal Frame preferences")]
    [SerializeField] private string label;
    [SerializeField] private string primaryButtonText;
    [SerializeField] private string secondaryButtonText;

    private void Start()
    {
        modalFrame.OnAnswer += (answer) =>
        {
            if (answer == ModalFrameButton.Primary)
                SceneManager.LoadScene(sceneIndex);
            else
                modalFrame.gameObject.SetActive(false);
        };
    }

    public void StartDialog()
    {
        modalFrame.SetLabel(label).
            SetPrimaryButton(primaryButtonText).
            SetSecondaryButton(secondaryButtonText);

        modalFrame.gameObject.SetActive(true);
    }
}