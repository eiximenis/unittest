using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public record TransferData(decimal amount, string concept);

    public interface IPaymentService
    {
        public bool Transfer(string from, string to, TransferData data);
    }
}
