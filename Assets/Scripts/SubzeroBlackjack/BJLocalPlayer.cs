using UnityEngine;
using UnityEngine.UI;

public class BJLocalPlayer : BJPlayer, IBJPlayerCallbacks
{
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    public override void StartMove(BJGameManager manager)
    {
        OnStartMove?.Invoke(this);

        trueButton.gameObject.SetActive(true);
        falseButton.gameObject.SetActive(true);

        trueButton.onClick.AddListener(() =>
        {
            manager.PlayerStep(this, BJStepState.GetCard);
        });

        falseButton.onClick.AddListener(() =>
        {
            manager.PlayerStep(this, BJStepState.Pass);
        });
    }

    public override void EndMove()
    {
        OnEndMove?.Invoke(this);

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
    }

    public override void TrumpChoose()
    {
        OnTrumpChoose?.Invoke(this);
    }
}