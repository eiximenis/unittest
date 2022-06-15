using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public interface IEmailService
    {
        public void SendEmail(string to, string subject, string body);
    }
}
