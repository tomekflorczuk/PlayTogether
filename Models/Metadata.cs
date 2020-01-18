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
        [Display(Name = "Logowanko")]
        [MaxLength(20, ErrorMessage = "Your login is too long, it should be maximum 20 characters long")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name= "Hasełko")]
        [MaxLength(256, ErrorMessage = "Your password is too long, it should be maximum 25 characters long ")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Adres E-mail")]
        [MaxLength(30, ErrorMessage = "Your e-mail is too long, it should be maximum 30 characters long")]
        public string Email { get; set; }
        
    }
    public class PlayersMetadata
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        [MaxLength(25)]
        public string LastName { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Ksywka")]
        [MaxLength(15)]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "O sobie")]
        [MaxLength(256)]
        public string Bio { get; set; }
    }
    public class GamesMetadata
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data wydarzenia")]
        public DateTime GameDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Czas trwania wydarzenia")]
        public TimeSpan GameLength { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Maksymalna ilość uczestników")]
        public int MaxPlayers { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Cena od osoby")]
        public sbyte? Price { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        [Display(Name = "Opis")]
        public string Notes { get; set; }
    }
}
