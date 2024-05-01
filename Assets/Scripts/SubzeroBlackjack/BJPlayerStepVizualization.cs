using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BJPlayerStepVizualization : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI localPlayerVizualize;
    [SerializeField] private TextMeshProUGUI externalPlayerVizualize;

    [SerializeField] private Animator localPlayerVizualizeAnimator;
    [SerializeField] private Animator externalPlayerVizualizeAnimator;

    [SerializeField] private GameObject localPlayerPointer;
    [SerializeField] private GameObject enemyPlayerPointer;

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

    public void OnStep(BJRequestData data)
    {
        bool state = data.UserSenderId == UserDataWrapper.UserData.Id.ToString();
    
        localPlayerPointer.SetActive(state);
        enemyPlayerPointer.SetActive(!state);
    }

    public void OnBetVizualize(BJRequestData data)
    {
        char betSymbol = int.Parse(data.Args[0]) > 0 ? '↑' : '↓';
        string strMessage = $"<color=#FFD701>Bet {betSymbol}</color>";

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

    public void OnGameEndVizualize(BJRequestData data)
    {
        string strMessage = "<color=#93CC2F>WIN!!</color>";

        if (data.UserSenderId == UserDataWrapper.UserData.Id.ToString())
        {
            StartCoroutine(MakeAfterDelay(
            () =>
            {
                localPlayerVizualize.text = strMessage;
                localPlayerVizualizeAnimator.SetTrigger("ShowText");
            }, 
            () =>
            {
                localPlayerVizualize.text = $"<color=#93CC2F>+{data.Args[0]}</color>";
                localPlayerVizualizeAnimator.SetTrigger("ShowText");
            }, 1.1f));
        }
        else
        {
            externalPlayerVizualize.text = strMessage;
            externalPlayerVizualizeAnimator.SetTrigger("ShowText");
        }
    }

    IEnumerator MakeAfterDelay(Action before, Action after, float delay)
    {
        before();

        yield return new WaitForSeconds(delay);

        after();
    }
}