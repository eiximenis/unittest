namespace Bank2.FraudTestApi
{
    public interface IFraudService
    {
        Task<bool> CheckFraud(string iban);
        Task DeleteFraud(string iban);
        Task SetFraud(string iban);
    }
}
