using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using MySqlX.XDevAPI.Relational;

namespace PlayTogether.Models
{
    public partial class Surfaces
    {
        public Surfaces()
        {
            Places = new HashSet<Places>();
        }

        [Key]
        public int SurfaceId { get; set; }
        public string SurfaceName { get; set; }
        public virtual ICollection<Places> Places { get; set; }
    }
}
