﻿namespace Bank.FraudApi
{
    public interface IFraudService
    {
        Task<bool> CheckFraud(string iban);
        Task DeleteFraud(string iban);
        Task SetFraud(string iban);
    }
}
