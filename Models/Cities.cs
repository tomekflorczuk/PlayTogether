using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Models
{
    public partial class Cities
    {
        public Cities()
        {
            Places = new HashSet<Places>();
        }

        [Key]
        public sbyte CityId { get; set; }
        public string CityName { get; set; }
        public virtual ICollection<Places> Places { get; set; }
    }
}
