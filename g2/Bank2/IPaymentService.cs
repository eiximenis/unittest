using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank2
{
    public record TransferData(decimal Amount, string Concept);
    
    public interface IPaymentService
    {
        bool Transfer(string ibanFrom, string ibanTo, TransferData data);
    }
}
