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

    public void WalletAddMoney(object sender, ulong amount)
    {
        ulong oldMoney = wallet.Money;

        wallet.AddMoney(amount);

        OnWalletMoneyChanged?.Invoke(new WalletOperationEventArgs
        (
            oldMoney,
            wallet.Money,
            $"{sender} add money on your balance",
            OperationStatus.Success,
            OperationType.AddMoney,
            sender,
            DateTime.Now
        ));
    }

    public void WalletGetMoney(object sender, ulong amount)
    {
        ulong oldMoney = wallet.Money;
        bool isSuccess = wallet.TryGetMoney(amount);

        OnWalletMoneyChanged?.Invoke(new WalletOperationEventArgs
        (
            oldMoney,
            wallet.Money,
            isSuccess ? $"{sender} get money from your balance" : $"{sender} can't get money from your balance",
            isSuccess ? OperationStatus.Success : OperationStatus.Denied,
            OperationType.GetMoney,
            sender,
            DateTime.Now
        ));
    }
}