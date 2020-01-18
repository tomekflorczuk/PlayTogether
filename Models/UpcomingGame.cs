using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Models
{
    public partial class UpcomingGame
    {
        public int HostUser { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameLength { get; set; }
        public sbyte GameType { get; set; }
        public int MaxPlayers { get; set; }
        public sbyte? Price { get; set; }
        public sbyte PlaceId { get; set; }
        public DateTime Created { get; set; }
        public string Notes { get; set; }
    }
}
