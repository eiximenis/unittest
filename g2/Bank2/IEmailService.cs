using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank2
{
    public interface IEmailService
    {
        void SendEmail(string to, string caption, string body);
    }
}
