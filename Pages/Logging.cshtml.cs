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
using System.Web;
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
                    var hashedpassword = Crypto.SHA256(Users.Password);
                    var logininfo = await _context.LoggingMethodAsync(Users.Login,
                        hashedpassword, Users.Email);

                    if (logininfo != null && logininfo.Any())
                    {
                        var logindetails = logininfo.First();
                        await SignInUser(logindetails.Login, false);
                        _session.LoggedId = logindetails.User_Id;
                        return RedirectToPage("/Main");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page();
        }
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
        //RegisterButton
        public async Task<IActionResult> OnPostSignUp()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginduplicate = await _context.Set<Logging>().FromSqlRaw("CALL CheckLoginDuplicate (@p0)", parameters: new[] {Users.Login}).ToListAsync();
                    var mailduplicate = await _context.Set<Logging>().FromSqlRaw("CALL CheckEmailDuplicate (@p0)", parameters: new[] {Users.Email}).ToListAsync();


                    if (loginduplicate.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Login is already in use");
                        return Page();
                    }

                    if (mailduplicate.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already in use");
                        return Page();
                    }

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Page();
        }
    }
}