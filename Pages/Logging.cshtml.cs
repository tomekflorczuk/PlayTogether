using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Models;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlayTogether.Data;

namespace PlayTogether.Pages
{
    public class LoggingModel : PageModel
    {
        private readonly PtContext _context;
        private readonly AppData _session;

        public LoggingModel(PtContext context, AppData session)
        {
            _context = context;
            _session = session;
        }

        [BindProperty] public Models.Users Users { get; set; }
        [BindProperty] public Players Players { get; set; }
        [BindProperty] public PasswordConfirmation passwords { get; set; }

        ///Checking if user is already logged in
        public IActionResult OnGet()
        {
            try
            {
                if (User.Identity.IsAuthenticated) return RedirectToPage("/Main");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page();
        }
        //Login button
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                if ((ModelState.GetFieldValidationState("Users.Login") == ModelValidationState.Valid || ModelState.GetFieldValidationState("Users.Email") == ModelValidationState.Valid) &
                    (ModelState.GetFieldValidationState("Users.Password") == ModelValidationState.Valid))
                {
                    //Hashowanie hasła
                    var hashedpassword = Crypto.SHA256(Users.Password);
                    //Pobieranie z bazy danych użytkowników z pasującymi danymi logowania
                    var macheduser = await _context.Users.SingleAsync(u => u.Login == Users.Login || u.Email == Users.Email);
                    //Sprawdzanie czy jest pasujący użytkownik
                    if (macheduser != null && macheduser.Password == hashedpassword)
                    {
                        //Sprawdzanie czy użytkownik ma aktywne konto
                        if (macheduser.UserStatus == 'A')
                        {
                            //Autoryzowanie użytkownika
                            await SignInUser(macheduser.Login, false);
                            _session.LoggedId = macheduser.UserId;
                            _session.SelectedSportType = 0;
                            _session.SelectedCity = 1;
                            return RedirectToPage("/Main");
                        }
                        ModelState.AddModelError(string.Empty, "Account is inactive");
                        return Page();
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unknown error");
            }

            return Page();
        }
        //Authorizing user
        private async Task SignInUser(string login, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, login));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    claimPrincipal, new AuthenticationProperties() {IsPersistent = isPersistent});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Register button
        public async Task<IActionResult> OnPostSignUp()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Checking for duplicates
                    if (await _context.Users.Where(p => p.Login == Users.Login).AnyAsync())
                    {
                        ModelState.AddModelError(string.Empty, "Login is already in use");
                        return Page();
                    }
                    if (await _context.Users.Where(p => p.Email == Users.Email).AnyAsync())
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already in use");
                        return Page();
                    }

                    try
                    {
                        _context.Players.Add(Players);
                        await _context.SaveChangesAsync();
                        var playerid = _context.Players.OrderByDescending(p => p.PlayerId).FirstOrDefault();
                        Users.Password = Crypto.SHA256(Users.Password);
                        Users.PlayerId = playerid.PlayerId;
                        _context.Users.Add(Users);
                        await _context.SaveChangesAsync();
                        await SignInUser(Users.Login, false);
                        _session.LoggedId = Users.UserId;
                        return RedirectToPage("/Main");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Error while saving to database");
                    }
                }
                ModelState.AddModelError(string.Empty,"Validation failed");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unknown error");
            }
            return Page();
        }
        //Reset password
        public async Task<IActionResult> OnPostResetPassword()
        {
            if (passwords.Password1 == passwords.Password2)
            {
                var newpassword = Crypto.SHA256(passwords.Password1);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Passwords are not the same");
            }

            return Page();
        }
    }
}