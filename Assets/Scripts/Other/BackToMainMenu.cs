using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(2);
    }
}
