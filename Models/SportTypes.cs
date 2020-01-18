using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using MySqlX.XDevAPI.Relational;

namespace PlayTogether.Models
{
    [Table("SportTypes")]
    public partial class SportTypes
    {
        public SportTypes()
        {
            Games = new HashSet<Games>();
        }

        [Key]
        public sbyte TypeId { get; set; }
        public string SportType { get; set; }

        public virtual ICollection<Games> Games { get; set; }
    }
}
