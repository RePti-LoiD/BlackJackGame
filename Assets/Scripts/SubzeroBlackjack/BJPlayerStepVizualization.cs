using TMPro;
using UnityEngine;

public class BJPlayerStepVizualization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI localPlayerVizualize;
    [SerializeField] private TextMeshProUGUI externalPlayerVizualize;

    [SerializeField] private Animator localPlayerVizualizeAnimator;
    [SerializeField] private Animator externalPlayerVizualizeAnimator;

    public void StepStateVizualize(BJRequestData data)
    {
        string strMessage = data.Args[0] == "GetCard" ? "<color=#FFD701>PICK!</color>" : "PASS";

        if (data.UserSenderId == UserDataWrapper.UserData.Id.ToString())
        {
            localPlayerVizualize.text = strMessage;
            localPlayerVizualizeAnimator.SetTrigger("ShowText");
        }
        else
        {
            externalPlayerVizualize.text = strMessage;
            externalPlayerVizualizeAnimator.SetTrigger("ShowText");
        }
    }

    public void OnBetVizualize(BJRequestData data)
    {
        //↓↑
        string strMessage = data.Args[0] == "GetCard" ? "<color=#FFD701>PICK!</color>" : "PASS";

        if (data.UserSenderId == UserDataWrapper.UserData.Id.ToString())
        {
            localPlayerVizualize.text = strMessage;
            localPlayerVizualizeAnimator.SetTrigger("ShowText");
        }
        else
        {
            externalPlayerVizualize.text = strMessage;
            externalPlayerVizualizeAnimator.SetTrigger("ShowText");
        }
    }
}