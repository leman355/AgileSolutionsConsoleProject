using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public Decimal Balance { get; set; }
        public Decimal CreditBalance { get; set; }
        public bool IsEmployed { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}
