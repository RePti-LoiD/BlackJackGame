using UnityEngine;

public class UserDataVizualizeInvoker : MonoBehaviour
{
    [SerializeField] private UserDataVisualization dataVisualization;

    private void Start() =>
        dataVisualization.VisualizeUserData(UserDataWrapper.UserData);
}