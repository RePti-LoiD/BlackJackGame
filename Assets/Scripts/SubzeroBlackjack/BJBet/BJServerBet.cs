public class BJServerBet : BJBet
{
    public void SetBet(int bet)
    {
        CurrentBet = bet;

        OnBetSet?.Invoke(CurrentBet);
    }
}
