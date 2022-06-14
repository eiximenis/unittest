namespace Bank3;
public class BankAccount
{
    public decimal Money { get; private set; }
    public bool IsBlocked { get; init; }


    public BankAccount()
    {
        Money = 0m;
        IsBlocked = false;
    }

    public BankAccount(decimal initialAmount)
    {
        Money = initialAmount;
        IsBlocked = false;
    }
    
    public void Deposit(decimal qty)
    {
        if (qty < 0) throw new ArgumentException("Negative ammounts not valid");
        Money += qty;
    }

    public void Withdraw(decimal qty)
    {
        Money -= qty;
    }
}
