using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PlayTogether.Models
{
    public class UsersMetadata
    {
        [Required(ErrorMessage = "Login is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [MaxLength(20, ErrorMessage = "Your login is too long, it should be maximum 20 characters long")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name= "Password")]
        [MaxLength(256, ErrorMessage = "Your password is too long, it should be maximum 25 characters long ")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "E-mail address")]
        [MaxLength(30, ErrorMessage = "Your e-mail is too long, it should be maximum 30 characters long")]
        public string Email { get; set; }

        [Display(Name = "Account created")]
        public DateTime Created { get; set; }

    }
    public class PlayersMetadata
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [MaxLength(25)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nickname")]
        [MaxLength(15)]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth day")]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "About you")]
        [MaxLength(256)]
        public string Bio { get; set; }

        //[DataType(DataType.ImageUrl)]
        [DataType(DataType.Upload)]
        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Points Of Trust")]
        public int PointsOfTrust { get; set; }

        [Display(Name = "Games Attended")]
        public int GamesAttended { get; set; }
    }
    public class GamesMetadata
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Game date")]
        public DateTime GameDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Game duration")]
        public TimeSpan GameLength { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Maximum number of players")]
        public int MaxPlayers { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Price per person")]
        public sbyte? Price { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Game created")]
        public DateTime Created { get; set; }
    }
    public class PlaceMetadata
    {
        [DataType(DataType.Text)]
        [Display(Name = "Place name")]
        public string PlaceName { get; set; }

        [Display(Name = "City")]
        public sbyte CityId { get; set; }

        [Display(Name = "Surface of the field")]
        public sbyte SurfaceId { get; set; }
    }
}
