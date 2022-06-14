﻿namespace Bank2;
public class BankAccount
{
    public decimal Money { get; private set; }

    public BankAccount()
    {
        Money = 0m;
    }

    public void Deposit(decimal qty)
    {
        if (qty < 0) throw new ArgumentException("Negative quantity not allowed");
        Money += qty;
    }
    
    public void Withdraw(decimal qty)
    {
        Money -= qty;
    }
}