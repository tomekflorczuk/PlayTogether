using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlayTogether.Models;
using Microsoft.EntityFrameworkCore;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using PlayTogether.Data;

namespace PlayTogether.Pages
{
    [Authorize]
    public class MainModel : PageModel
    {
        private readonly PtContext _context;
        private readonly AppData _session;
        private readonly CloudStorage _bucket;
        private readonly StorageClient _storage;
        //Constructor
        public MainModel(PtContext context, AppData session, IOptions<CloudStorage> bucket)
        {
            _context = context;
            _session = session;
            _bucket = bucket.Value;
            _storage = StorageClient.Create();
        }
        //Properties
        [BindProperty] public ProfilePicture ProfilePicture { get; set; }
        [BindProperty] public List<UpcomingGame> UpcomingGames { get; set; }
        [BindProperty] public List<Cities> Cities { get; set; }
        [BindProperty] public List<Surfaces> Surfaces { get; set; }
        [BindProperty] public List<Places> Places { get; set; }
        [BindProperty] public Games NewGame { get; set; }
        [BindProperty] public Players Player { get; set; }
        [BindProperty] public Places NewPlace { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Player = await _context.Players.Include(u => u.Users).FirstOrDefaultAsync(m => m.PlayerId == _session.LoggedId);
            UpcomingGames = await _context.UpcomingGames.ToListAsync();
            Cities = await _context.ListCities.ToListAsync();
            Surfaces = await _context.ListSurfaces.ToListAsync();
            //Places = await _context.Set<Places>().FromSqlRaw("Call ListPlaces(@p0)", new[] { }).ToListAsync();
            /*
            try
            {
                var uploadedpicture = await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                ProfilePicture.PictureUrl = uploadedpicture.MediaLink;
            }
            catch
            {
                ProfilePicture.PictureUrl = null;
            }
            */
            ViewData["SelectCitiesTop"] = new SelectList(Cities, "CityName", "CityName");
            ViewData["SelectCities"] = new SelectList(Cities, "CityId", "CityName");
            ViewData["SelectSurfaces"] = new SelectList(Surfaces, "SurfaceId", "SurfaceName");

            return Page();
        }
        //Clicking FootballButton
        public void OnPostFootballButton()
        {
            _session.SelectedSportType = 1;
        }
        //Clicking BasketballButton
        public void OnPostBasketballButton()
        {
            _session.SelectedSportType = 2;
        }
        //Clicking VolleyballButton
        public void OnPostVolleyballButton()
        {
            _session.SelectedSportType = 3;
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
            NewGame.HostUser = _session.LoggedId;
            //Game.GameType = 

            return Page();
        }
        //AddPlaceButton
        public async Task<IActionResult> OnPostAddPlace()
        {
            if (ModelState.GetFieldValidationState("NewPlace.PlaceName") == ModelValidationState.Valid)
            {
                try
                {
                    _context.Places.Add(NewPlace);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid input");
            }

            return Page();
        }
        //ChangeUserDetails
        public async Task<IActionResult> OnPostUserDetails()
        {
            try
            {
                //Checking validation
                if (ModelState.IsValid)
                {
                    //Checking if image was send
                    if (ProfilePicture.PictureFile == null)
                    {
                        _context.Attach(Player).State = EntityState.Modified;
                        try
                        {
                            //Updating data in database
                            Player.Modified = DateTime.Now;
                            await _context.SaveChangesAsync();
                            return RedirectToPage("/Index");
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            throw ex;
                        }
                    }
                    //Processing picture send from view
                    IFormFile picture = ProfilePicture.PictureFile;
                    if (picture.ContentType.Contains("image"))
                    {
                        Stream pictureStream = picture.OpenReadStream();
                        Google.Apis.Storage.v1.Data.Object uploadedImage;
                        try
                        {
                            //Deleting existing picture
                            await _storage.DeleteObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                            //Uploading picture to cloud storage
                            await _storage.UploadObjectAsync(_bucket.BucketName, Player.PlayerId.ToString(),
                                picture.ContentType, pictureStream);
                            uploadedImage =
                                await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                        }
                        catch (Exception ex)
                        {
                            //Uploading picture to cloud storage
                            await _storage.UploadObjectAsync(_bucket.BucketName, Player.PlayerId.ToString(),
                                picture.ContentType, pictureStream);
                            uploadedImage =
                                await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                        }

                        _context.Attach(Player).State = EntityState.Modified;
                        
                        try
                        {
                            //Updating data in database
                            Player.Modified = DateTime.Now;
                            Player.ProfilePicture = uploadedImage.MediaLink;
                            //_session.PictureUrl = uploadedImage.MediaLink;
                            await _context.SaveChangesAsync();
                            return Page();
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Page();
        }
    }
}