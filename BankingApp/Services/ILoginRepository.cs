using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Services
{
    public interface ILoginRepository
    {
        Task<int> Login(string email, string password);
    }
}
