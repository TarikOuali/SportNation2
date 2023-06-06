using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SportNation2.Data;
using SportNation2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportNation2.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor accessor;
        private readonly AppDbContext dbContext;

        public AccountService(IHttpContextAccessor accessor, AppDbContext dbContext)
        {
            this.accessor = accessor;
            this.dbContext = dbContext;
        }
        public async Task LoginAsync(string email, string password, bool rememberMe)
        {
            //création de cookie avec claims
            //- Trouver le User dans la bdd
            var user = dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == email);

            if (user is null)
            {
                throw new Exception("Not found");
            }
            if (!await Helpers.IsPasswordCorrect(password, user.HashedPassword))
            {
                throw new Exception("Incorrect credientials");
            }
            //- créer une liste de Claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Gender, user.Genre.ToString()),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString("O")),
                new Claim(ClaimTypes.Email, user.Email),
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            //- Créer un objet ClaimsIdentity
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //- Créer un objet ClaimsPrincipal
            var principal = new ClaimsPrincipal(identity);

            //- Utiliser HttpContext pour le SignIn du principal
            await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties()
                {
                    IsPersistent = rememberMe
                });
        }

        public async Task LogoutAsync()
        {
            if (accessor.HttpContext!.User.Identity!.IsAuthenticated)
            {
                await accessor.HttpContext.SignOutAsync();
            }
        }
    }
}
