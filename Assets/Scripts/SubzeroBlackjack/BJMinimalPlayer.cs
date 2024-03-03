
public class BJMinimalPlayer : BJPlayer
{
    public override void EndMove() => 
        OnEndMove?.Invoke(this);

    public override void StartMove(BJGameManager manager)
    {
        OnStartMove?.Invoke(this);
        //manager.PlayerStep(this, 0);
    } 


    public override void TrumpChoose() => 
        OnTrumpChoose?.Invoke(this);
}