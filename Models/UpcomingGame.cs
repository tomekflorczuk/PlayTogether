using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Models
{
    public partial class UpcomingGame
    {
        public int GameId { get; set; }
        public int PlaceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameLength { get; set; }
        public int GameType { get; set; }
        public int MaxPlayers { get; set; }
        public int? Price { get; set; }
        public DateTime Created { get; set; }
        public string Notes { get; set; }
        public DateTime Modified { get; set; }
        public string PlaceName { get; set; }
        public string CityName { get; set; }
        public string SurfaceName { get; set; }
    }
}
