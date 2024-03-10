using TMPro;
using UnityEngine;

public class StatisticsFrameUserDataUpdater : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickName;
    [SerializeField] private TMP_InputField firstName;
    [SerializeField] private UserDataVisualization visualization;

    private void Awake()
    {
        nickName.onEndEdit.AddListener((text) =>
        {
            UserDataWrapper.UserData.NickName = text;
            UserDataWrapper.SaveData();

            visualization.UpdateData();
        });
        
        firstName.onEndEdit.AddListener((text) =>
        {
            UserDataWrapper.UserData.FirstName = text;
            UserDataWrapper.SaveData();

            visualization.UpdateData();
        });
    }
}