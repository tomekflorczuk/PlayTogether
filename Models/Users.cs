using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PlayTogether.Models
{
    public partial class Users
    {
        public Users()
        {
        }

        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime Modified { get; set; }
        public char UserStatus { get; set; }
        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public virtual Players Player { get; set; }
        [ForeignKey("RoleId")] 
        public virtual Roles Role { get; set; }
    }
}
