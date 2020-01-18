using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using MySqlX.XDevAPI.Relational;

namespace PlayTogether.Models
{
    [Table("Roles")]
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        [Key]
        public sbyte RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
