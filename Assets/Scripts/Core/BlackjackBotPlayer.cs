using System;

public class BlackjackBotPlayer : BlackjackPlayer
{
    protected override void Start() { }

    public override void OnTurn(BlackjackCardManager cardManager)
    {
        int currentScore = CalculateScore();
        bool isRisk = Convert.ToBoolean(UnityEngine.Random.Range(0, 1));

        if (currentScore < cardManager.MaxBlackjackScore && (cardManager.MaxBlackjackScore - currentScore > 4 || isRisk))
            turnState = TurnState.Hit;
        else
            turnState = TurnState.Stand;
    }

    public override TurnState OnTurnEnd()
    {
        return turnState;
    }

    protected override void ShowScore(int score)
    {
        scoreText.text = $"X + {score - cards[0].CardWeight}";
    }
}