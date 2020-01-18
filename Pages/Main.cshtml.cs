using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PlayTogether.Models;
using Microsoft.EntityFrameworkCore;

namespace PlayTogether.Pages
{
    [Authorize]
    public class MainModel : PageModel
    {
        private readonly Data.PtContext _context;

        public MainModel(Data.PtContext context)
        {
            _context = context;
        }

        private int _loggedid;

        private int choosedsporttype;

        public List<UpcomingGame> UpcomingGames { get; set; }
        [BindProperty] public Games Game { get; set; }
        [BindProperty] public Players Player { get; set; }

        public async Task<IActionResult> OnGetAsync(int loggedid)
        {
            _loggedid = loggedid;
            Player = _context.Players.Single(p => p.PlayerId == loggedid);

            UpcomingGames = new List<UpcomingGame>();
            UpcomingGames = await _context.UpcomingGames.ToListAsync(); 
            /*Player = _context.Players.Join(
                _context.Users,
                player => player.PlayerId,
                user => user.PlayerId,
                (player, user) => new
                {
                    FirstName = player.FirstName,
                    LastName = player.LastName,

                }).FirstOrDefault();

            */
            return Page();
        }

        //Clicking FootballButton
        public void OnPostFootballButton()
        {
            choosedsporttype = 1;
        }
        
        //Clicking BasketballButton
        public void OnPostBasketballButton()
        {
            choosedsporttype = 2;
        }
        
        //Clicking VolleyballButton
        public void OnPostVolleyballButton()
        {
            choosedsporttype = 3;
        }
        
        //SignOutButton
        public async Task<IActionResult> OnPostLogOut()
        {
            try
            {
                var authenticationmanager = Request.HttpContext;
                await authenticationmanager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToPage("/Logging");
        }

        //AddEventButton
        public async Task<IActionResult> OnPostAddEvent()
        {
            Game.HostUser = _loggedid;
            return Page();
        }

        //ChangeUserDetails
        public async Task<IActionResult> OnPostUserDetails()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Player.Modified = DateTime.Now;
                    _context.Attach(Player).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Page();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
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