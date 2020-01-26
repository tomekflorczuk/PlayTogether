using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Models
{
    public partial class Players
    {
        public Players()
        {
            Users = new HashSet<Users>();
            Games = new HashSet<Games>();
            Participants = new HashSet<Participants>();
        }

        [Key]
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public DateTime BirthDate { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public int PointsOfTrust { get; set; }
        public int GamesAttended { get; set; }
        public DateTime Modified { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Games> Games { get; set; }
        public virtual ICollection<Participants> Participants { get; set; }
    }
}
