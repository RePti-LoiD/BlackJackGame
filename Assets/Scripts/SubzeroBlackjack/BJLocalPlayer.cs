using UnityEngine;
using UnityEngine.UI;

public class BJLocalPlayer : BJPlayer, IBJPlayerCallbacks
{
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    protected BJGameManager currentManager;

    protected void Start()
    {
        trueButton.onClick.AddListener(() =>
        {
            currentManager.PlayerStep(this, BJStepState.GetCard);
        });

        falseButton.onClick.AddListener(() =>
        {
            currentManager.PlayerStep(this, BJStepState.Pass);
        });
    }

    public override void StartMove(BJGameManager manager)
    {
        OnStartMove?.Invoke(this);

        trueButton.gameObject.SetActive(true);
        falseButton.gameObject.SetActive(true);

        currentManager = manager;
    }

    public override void EndMove()
    {
        OnEndMove?.Invoke(this);

        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
    }

    public override void TrumpChoose()
    {
        OnTrumpChoose?.Invoke(this);
    }
}