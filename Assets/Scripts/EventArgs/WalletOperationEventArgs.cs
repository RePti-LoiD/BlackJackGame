using System;

public struct WalletOperationEventArgs
{
    public ulong OldMoney, CurrentMoney;
    public string OperationDescription; 

    public OperationStatus IsOperationSuccess;
    public OperationType OperationType;
    public object Sender;

    public DateTime DateTime;

    public WalletOperationEventArgs(ulong oldMoney, ulong currentMoney, string description, OperationStatus status, OperationType type, object sender, DateTime dateTime)
    {
        OldMoney = oldMoney;
        CurrentMoney = currentMoney;

        OperationDescription = description;
        IsOperationSuccess = status;
        OperationType = type;
        Sender = sender;

        DateTime = dateTime;
    }
}
