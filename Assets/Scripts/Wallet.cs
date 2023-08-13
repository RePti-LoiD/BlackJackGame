using UnityEngine;

[CreateAssetMenu(fileName = "BlackJackObj", menuName = "Wallet")]
public class Wallet : ScriptableObject
{
    [SerializeField] private ulong money;
    public ulong Money => money;

    public delegate void WalletMoneyChanged();
    public event WalletMoneyChanged OnWalletMoneyChanged;

    public void AddMoney(ulong count)
    {
        money += count;

        OnWalletMoneyChanged?.Invoke();
    }

    public bool TryGetMoney(ulong count)
    {
        if (money - count > 0) return false;

        money -= count;
        
        OnWalletMoneyChanged?.Invoke();

        return true;
    }
}