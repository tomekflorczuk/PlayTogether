using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Models
{
    public partial class Places
    {
        public Places()
        {
            Games = new HashSet<Games>();
        }

        [Key]
        public sbyte PlaceId { get; set; }
        public string PlaceName { get; set; }
        public sbyte CityId { get; set; }
        public sbyte SurfaceId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        [ForeignKey("SurfaceId")]
        public virtual Surfaces Surface { get; set; }
        [ForeignKey("CityId")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Games> Games { get; set; }
    }
}
