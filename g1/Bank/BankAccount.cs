using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccount
    {
        public decimal Money { get; private set; }
        
        public bool IsBlocked { get; init; }

        public BankAccount()
        {
            Money = 0m;
            IsBlocked = false;
        }
        public BankAccount(decimal initialQty)
        {
            Money = initialQty;
            IsBlocked = false;
        }
        
        public void Ingress(decimal qty)
        {
            if (qty < 0) throw new ArgumentException("No negative ammounts allowed");
            Money += qty;
        }

        public void Withdrawn(decimal qty)
        {
            Money -= qty;
        }
        
    }
}
