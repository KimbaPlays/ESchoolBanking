using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models
{
    public class UserInfo
    {
        public int UserId { get; set; } //USER ID (NUMERIC)

        public string email { get; set; } //USER EMAIL
        public string password { get; set; } //USER PASSWORD
    }
}
