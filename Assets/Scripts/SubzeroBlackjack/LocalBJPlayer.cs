using UnityEngine;
using UnityEngine.UI;

public class LocalBJPlayer : BJPlayer
{
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    public override void StartMove(BJGameManager manager)
    {
        trueButton.gameObject.SetActive(true);
        falseButton.gameObject.SetActive(true);

        trueButton.onClick.AddListener(() =>
        {
            manager.PlayerStep(this, true);
        });

        falseButton.onClick.AddListener(() =>
        {
            manager.PlayerStep(this, false);
        }); ;
    }

    public override void EndMove()
    {
        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
    }

    public override void TrumpChoose()
    {
        throw new System.NotImplementedException();
    }
}