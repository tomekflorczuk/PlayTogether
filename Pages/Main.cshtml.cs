using System;
using System.Collections.Generic;
using System.IO;
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
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

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

        [BindProperty] private int Loggedid { get; set; }
        [BindProperty] private int choosedsporttype { get; set; }
        [BindProperty] public ProfilePicture ProfilePicture { get; set; }

        private string[] extensions = {".jpg", ".png"};
        public string Result { get; set; }
        [BindProperty] public List<UpcomingGame> UpcomingGames { get; set; }
        [BindProperty] public Games Game { get; set; }
        [BindProperty] public Players Player { get; set; }

        public async Task<IActionResult> OnGetAsync(int loggedid)
        {
            Loggedid = loggedid;
            Player = _context.Players.Single(p => p.PlayerId == loggedid);

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
            Game.HostUser = Loggedid;
            return Page();
        }

        //ChangeUserDetails
        public async Task<IActionResult> OnPostUserDetails()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IFormFile picture = ProfilePicture.PictureFile;
                    if (picture.ContentType.Contains("image"))
                    {
                        _context.Attach(Player).State = EntityState.Modified;

                        var storage = StorageClient.Create();

                        try
                        {
                            Player.Modified = DateTime.Now;
                            await _context.SaveChangesAsync();
                            return Page();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        Result = "Invalid type of file";
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