using System;

public class BlackjackBotPlayer : BlackjackPlayer
{
    protected override void Start() { }

    public override void OnTurn(BlackjackCardManager cardManager)
    {
        int currentScore = CalculateScore();
        bool isRisk = Convert.ToBoolean(UnityEngine.Random.Range(0, 1));

        if (currentScore < cardManager.MaxBlackjackScore || (cardManager.MaxBlackjackScore - currentScore > 4 && isRisk))
            turnState = TurnState.Hit;
        else
            turnState = TurnState.Stand;
    }

    public override TurnState OnTurnEnd()
    {
        return turnState;
    }

    protected override int CalculateScore()
    {
        int score = 0;

        for (int i = 1; i < cards.Count; i++)
            score += cards[i].CardWeight;

        return score;
    }

    protected override void ShowScore(string scoreText)
    {
        this.scoreText.text = $"X + {scoreText}";
    }
}