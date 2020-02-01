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
        [MinLength(1)]
        [MaxLength(20, ErrorMessage = "Your login is too long, it should be maximum 20 characters long")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name= "Password")]
        [MinLength(1)]
        [MaxLength(25, ErrorMessage = "Your password is too long, it should be maximum 25 characters long ")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression("^([a-zA-Z0-9_\\.\\-]+)@([a-zA-Z0-9-]+.)[a-zA-Z]{2,6}$", 
            ErrorMessage = "Invalid format of email address")]
        [Display(Name = "E-mail address")]
        [MinLength(5)]
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
        [MinLength(1)]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [MinLength(1)]
        [MaxLength(25)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Nickname")]
        [MaxLength(15)]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date")]
        [Display(Name = "Birth day")]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "About you")]
        [MaxLength(256)]
        public string Bio { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Points Of Trust")]
        public int PointsOfTrust { get; set; }

        [Display(Name = "Games Attended")]
        public int GamesAttended { get; set; }
    }
    public class GamesMetadata
    {
        [Required(ErrorMessage = "Game date is required")]
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
        public int? Price { get; set; }

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
        [MinLength(5)]
        [MaxLength(50)]
        public string PlaceName { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "Surface of the field")]
        public int SurfaceId { get; set; }
    }
    public class PasswordsConfirmationMetadata
    {/*
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [MinLength(1)]
        [MaxLength(25, ErrorMessage = "Your password is too long, it should be maximum 25 characters long ")]
        public string Password1 { get; set; }
        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat new password")]
        [MinLength(1)]
        [MaxLength(25, ErrorMessage = "Your password is too long, it should be maximum 25 characters long ")]
        public string Password2 { get; set; }
    */
        }
}
