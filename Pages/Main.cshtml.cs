using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Models;
using PlayTogether.Data;
using Google.Cloud.Storage.V1;

namespace PlayTogether.Pages
{
    [Authorize]
    public class MainModel : PageModel
    {
        public readonly PtContext _context;
        public readonly AppData _session;
        private readonly CloudStorage _bucket;
        private readonly StorageClient _storage;

        //Constructor
        public MainModel(PtContext context, AppData session, IOptions<CloudStorage> bucket)
        {
            _context = context;
            _session = session;
            _bucket = bucket.Value;
            var credential = GoogleCredential.FromFile("google_credentials.json");
            _storage = StorageClient.Create(credential);
        }

        //Properties
        [BindProperty] public Players Player { get; set; }
        [BindProperty] public List<UpcomingGame> UpcomingGames { get; set; }
        [BindProperty] public List<Cities> Cities { get; set; }
        [BindProperty] public List<Surfaces> Surfaces { get; set; }
        [BindProperty] public List<Places> Places { get; set; }
        [BindProperty] public Games NewGame { get; set; }
        [BindProperty] public Places NewPlace { get; set; }
        [BindProperty] public Places SelectedPlace { get; set; }

        //On page load
        public async Task<IActionResult> OnGetAsync(AppData session)
        {
            Player = await _context.Players.Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.PlayerId == _session.LoggedId);
            Cities = await _context.ListCities.ToListAsync();
            Surfaces = await _context.ListSurfaces.ToListAsync();
            UpcomingGames = await _context.Set<UpcomingGame>()
                .FromSqlRaw("CALL UpcomingGames (@p0, @p1)", _session.SelectedSportType, _session.SelectedCity)
                .ToListAsync();
            Places = await _context.Set<Places>().FromSqlRaw("CALL ListPlaces(@p0)", _session.SelectedCity)
                .ToListAsync();

            ViewData["SelectPlace"] = new SelectList(Places, "PlaceId", "PlaceName");
            ViewData["SelectCitiesTop"] = new SelectList(Cities, "CityId", "CityName");
            ViewData["SelectCities"] = new SelectList(Cities, "CityId", "CityName");
            ViewData["SelectSurfaces"] = new SelectList(Surfaces, "SurfaceId", "SurfaceName");

            return Page();
        }

        //On city changed
        public async Task<JsonResult> OnPostCityChanged(int cityid)
        {
            _session.SelectedCity = cityid;
            //UpcomingGames = await _context.Set<UpcomingGame>().FromSqlRaw("CALL UpcomingGames (@p0, @p1)", _session.SelectedSportType, _session.SelectedCity).ToListAsync();
            //Places = await _context.Set<Places>().FromSqlRaw("CALL ListPlaces(@p0)", _session.SelectedCity).ToListAsync();

            //return new JsonResult(Places);
            return new JsonResult("City changed");
        }

        //Clicking FootballButton
        public async Task<JsonResult> OnPostFootballButton()
        {
            _session.SelectedSportType = 1;
            UpcomingGames = await _context.Set<UpcomingGame>()
                .FromSqlRaw("CALL UpcomingGames (@p0, @p1)", _session.SelectedSportType, _session.SelectedCity)
                .ToListAsync();
            return new JsonResult("Chosen football");
        }

        //Clicking BasketballButton
        public async Task<JsonResult> OnPostBasketballButton()
        {
            _session.SelectedSportType = 2;
            UpcomingGames = await _context.Set<UpcomingGame>()
                .FromSqlRaw("CALL UpcomingGames (@p0, @p1)", _session.SelectedSportType, _session.SelectedCity)
                .ToListAsync();
            return new JsonResult("Chosen basketball");
        }

        //Clicking VolleyballButton
        public async Task<JsonResult> OnPostVolleyballButton()
        {
            _session.SelectedSportType = 3;
            UpcomingGames = await _context.Set<UpcomingGame>()
                .FromSqlRaw("CALL UpcomingGames (@p0, @p1)", _session.SelectedSportType, _session.SelectedCity)
                .ToListAsync();
            return new JsonResult("Chosen volleyball");
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

        //SelectPlaceButton
        public async Task<JsonResult> OnPostSelectPlace(Places selectedplace)   
        {
            _session.SelectedPlace = selectedplace;
            return new JsonResult(new {placename = selectedplace.PlaceName, message = "Wybrano miejsce wydarzenia" });
        }

        //AddEventButton
        public async Task<JsonResult> OnPostAddEvent(Games newgame)
        {
            if (_session.SelectedSportType != 0)
            {
                if (ModelState.GetFieldValidationState("NewGame.GameDate") == ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("NewGame.GameLength") == ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("NewGame.Notes") == ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("NewGame.MaxPlayers") == ModelValidationState.Valid &&
                    ModelState.GetFieldValidationState("NewGame.Price") == ModelValidationState.Valid)
                {
                    NewGame.HostUser = _session.LoggedId;
                    NewGame.GameType = _session.SelectedSportType;
                    NewGame.PlaceId = _session.SelectedPlace.PlaceId;
                    try
                    {
                        _context.Games.Add(NewGame);
                        await _context.SaveChangesAsync();
                        return new JsonResult("Game has been added");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Validation failed");
                        return new JsonResult("Error while saving to database");
                    }
                }

                return new JsonResult("Validation failed");
            }

            return new JsonResult("Select sport type");
        }

        //AddPlaceButton
        public async Task<JsonResult> OnPostAddPlace(Places newplace)
        {
            try
            {
                if (ModelState.GetFieldValidationState("NewPlace.PlaceName") == ModelValidationState.Valid)
                    try
                    {
                        _context.Places.Add(NewPlace);
                        await _context.SaveChangesAsync();
                        return new JsonResult("Place added");
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Error while saving to database");
                    }

                ModelState.AddModelError(string.Empty, "Invalid input");
            }
            catch (Exception ex)
            {
                return new JsonResult("Unknown error");
            }

            return new JsonResult("");
        }

        //ChangeUserDetails
        public async Task<JsonResult> OnPostUserDetails(Players player, IFormFile picture)
        {
            try
            {
                if (!ModelState.IsValid) return new JsonResult("Invalid data");
                Google.Apis.Storage.v1.Data.Object uploadedimage;
                try
                {
                    //Sprawdzanie czy istnieje już zdjęcie profilowe dla tego użytkownika w cloud storage
                    uploadedimage = await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                    //Jeśli nie przesłano zdjęcia profilowego
                    if (picture == null)
                    {
                        _context.Attach(Player).State = EntityState.Modified;
                        //Aktualizacja danych w bazie danych
                        try
                        {
                            Player.Modified = DateTime.Now;
                            Player.ProfilePicture = uploadedimage.MediaLink;
                            await _context.SaveChangesAsync();
                            return new JsonResult("Your details has been updated");
                        }
                        catch (DbUpdateConcurrencyException dbex)
                        {
                            return new JsonResult("Error while saving to database");
                        }
                    }

                    //Jeśli przesłano nowe zdjęcie profilowe
                    if (picture.ContentType.Contains("image"))
                    {
                        //Usuwanie aktualnego zdjęcia profilowego z cloud storage
                        await _storage.DeleteObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());
                        //Wysłanie nowego zdjęcie profilowego do cloud storage
                        var pictureStream = picture.OpenReadStream();
                        await _storage.UploadObjectAsync(_bucket.BucketName, Player.PlayerId.ToString(),
                            picture.ContentType, pictureStream);
                        //Pobranie danych nowego zdjęcia z cloud storage
                        uploadedimage =
                            await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());

                        _context.Attach(Player).State = EntityState.Modified;
                        //Aktualizacja danych w bazie danych
                        try
                        {
                            Player.Modified = DateTime.Now;
                            Player.ProfilePicture = uploadedimage.MediaLink;
                            await _context.SaveChangesAsync();
                            return new JsonResult("Your details has been updated");
                        }
                        catch (DbUpdateConcurrencyException dbex)
                        {
                            return new JsonResult("Error while saving to database");
                        }
                    }

                    //Podano nie prawidłowy rodzaj pliku
                    return new JsonResult("Invalid type of file");
                }
                //Jeśli w nie istnieje zdjęcie profilowe dla tego użytkownika w cloud storage
                catch (Exception ex)
                {
                    //Jeśli nie przesłano zdjęcia profilowego
                    if (picture == null)
                    {
                        _context.Attach(Player).State = EntityState.Modified;
                        //Aktualizacja danych w bazie danych
                        try
                        {
                            Player.Modified = DateTime.Now;
                            await _context.SaveChangesAsync();
                            return new JsonResult("Your details has been updated");
                        }
                        catch (DbUpdateConcurrencyException dbex)
                        {
                            return new JsonResult("Error while saving to database");
                        }
                    }

                    //Jeśli przesłano nowe zdjęcie profilowe
                    if (picture.ContentType.Contains("image"))
                    {
                        //Wysłanie nowego zdjęcie profilowego do cloud storage
                        var pictureStream = picture.OpenReadStream();
                        await _storage.UploadObjectAsync(_bucket.BucketName, Player.PlayerId.ToString(),
                            picture.ContentType, pictureStream);
                        //Pobranie danych nowego zdjęcia z cloud storage
                        uploadedimage =
                            await _storage.GetObjectAsync(_bucket.BucketName, Player.PlayerId.ToString());

                        _context.Attach(Player).State = EntityState.Modified;
                        //Aktualizacja danych w bazie danych
                        try
                        {
                            Player.Modified = DateTime.Now;
                            Player.ProfilePicture = uploadedimage.MediaLink;
                            await _context.SaveChangesAsync();
                            return new JsonResult("Your details has been updated");
                        }
                        catch (DbUpdateConcurrencyException dbex)
                        {
                            return new JsonResult("Error while saving to database");
                        }
                    }

                    //Podano nie prawidłowy rodzaj pliku
                    return new JsonResult("Invalid type of file");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Request failed");
            }
        }

        //Sign up to a game
        public async Task<JsonResult> OnPostGameSignUp(int gameid)
        {
            var participant = await _context.Participants
                .Where(p => p.PlayerId == _session.LoggedId && p.GameId == gameid).FirstOrDefaultAsync();

            if (participant != null)
            {
                if (participant.ParticipantStatus == "U")
                {
                    _context.Attach(participant).State = EntityState.Modified;
                    try
                    {
                        participant.ParticipantStatus = "S";
                        await _context.SaveChangesAsync();
                        return new JsonResult("You were signed up to the game");
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult("Error while saving to database");
                    }
                }

                return new JsonResult("You are already signed to this game");
            }

            var newparticiapant = new Participants();
            newparticiapant.PlayerId = _session.LoggedId;
            newparticiapant.GameId = gameid;
            try
            {
                _context.Participants.Add(newparticiapant);
                await _context.SaveChangesAsync();
                return new JsonResult("You were signed up to the game");
            }
            catch (Exception ex)
            {
                return new JsonResult("Error while saving to database");
            }
        }

        //Sign out of a game
        public async Task<JsonResult> OnPostGameSignOut(int gameid)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.PlayerId == _session.LoggedId);
            _context.Attach(participant).State = EntityState.Modified;
            try
            {
                participant.ParticipantStatus = "U";
                await _context.SaveChangesAsync();
                return new JsonResult("You were signed out of the game");
            }
            catch (Exception ex)
            {
                return new JsonResult("Error while saving to database");
            }
        }
    }
}