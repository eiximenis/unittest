namespace Bank2
{
    public interface IBankAccountRepository
    {
        BankAccount GetByIban(string iban);
    }
}