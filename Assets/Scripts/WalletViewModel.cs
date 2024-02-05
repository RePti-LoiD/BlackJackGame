using System;
using UnityEngine;

public class WalletViewModel : MonoBehaviour
{
    [SerializeField] private Wallet wallet;

    public delegate void WalletMoneyChanged(WalletOperationEventArgs args);
    public event WalletMoneyChanged OnWalletMoneyChanged;

    private void Start ()
    {
        if (wallet == null) enabled = false;
    }

    public void WalletAddMoney(object sender, long amount)
    {
        long oldMoney = wallet.Balance;

        wallet.AddMoney(amount);

        OnWalletMoneyChanged?.Invoke(new WalletOperationEventArgs
        (
            oldMoney,
            wallet.Balance,
            $"{sender} add money on your balance",
            OperationStatus.Success,
            OperationType.AddMoney,
            sender,
            DateTime.Now
        ));
    }

    public void WalletGetMoney(object sender, long amount)
    {
        long oldMoney = wallet.Balance;
        bool isSuccess = wallet.TryGetMoney(amount);

        OnWalletMoneyChanged?.Invoke(new WalletOperationEventArgs
        (
            oldMoney,
            wallet.Balance,
            isSuccess ? $"{sender} get money from your balance" : $"{sender} can't get money from your balance",
            isSuccess ? OperationStatus.Success : OperationStatus.Denied,
            OperationType.GetMoney,
            sender,
            DateTime.Now
        ));
    }
}