public class Wallet
{
    private long balance;
    public long Balance => balance;

    public delegate void WalletMoneyChanged(long balance);
    public event WalletMoneyChanged OnWalletMoneyChanged;

    public void AddMoney(long count)
    {
        balance += count;

        OnWalletMoneyChanged?.Invoke(balance);
    }

    public bool TryGetMoney(long count)
    {
        if (balance - count < 0) return false;

        balance -= count;
        
        OnWalletMoneyChanged?.Invoke(balance);

        return true;
    }

    public Wallet(long balance)
    {
        this.balance = balance;
    }
}