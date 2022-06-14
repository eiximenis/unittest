using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank3
{
    public record TransferData(decimal Amount, string Concept);

    public enum TransferResult
    {
        Ok,
        NotEnoughFunds,
        BlockedAccount
    };
    
    public interface IPaymentsService
    {
        TransferResult Transfer(string ibanFrom, string ibanTo, TransferData data);
    }
}
