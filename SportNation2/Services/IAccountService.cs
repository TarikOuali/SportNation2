using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportNation2.Services
{
    public interface IAccountService
    {
        Task LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
    }
}
