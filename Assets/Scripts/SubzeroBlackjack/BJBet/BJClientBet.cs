public class BJClientBet : BJBet
{
    public void ChangeBet(int multiplier)
    {
        CurrentBet *= multiplier;

        OnBetSet?.Invoke(CurrentBet);
    }
}