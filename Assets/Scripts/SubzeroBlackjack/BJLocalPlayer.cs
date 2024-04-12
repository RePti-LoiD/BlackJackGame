using UnityEngine;
using UnityEngine.UI;

public class BJLocalPlayer : BJPlayer, IBJPlayerCallbacks
{
    [SerializeField] private Button trueButton;
    [SerializeField] private Button falseButton;

    protected BJGameManager currentManager;

    protected void Start()
    {
        trueButton.onClick.AddListener(PickCard);

        falseButton.onClick.AddListener(PassCard);
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

    public void PickCard() => 
        currentManager.PlayerStep(this, BJStepState.GetCard);

    public void PassCard() =>
        currentManager.PlayerStep(this, BJStepState.Pass);
}