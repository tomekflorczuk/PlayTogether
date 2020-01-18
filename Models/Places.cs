using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using MySqlX.XDevAPI.Relational;

namespace PlayTogether.Models
{
    [Table("Places")]
    public partial class Places
    {
        public Places()
        {
            Games = new HashSet<Games>();
        }

        [Key]
        public sbyte PlaceId { get; set; }
        public string PlaceName { get; set; }
        public sbyte SurfaceId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [ForeignKey("SurfaceId")]
        public virtual Surfaces Surface { get; set; }
        public virtual ICollection<Games> Games { get; set; }
    }
}
