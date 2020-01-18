using System;
using System.Collections.Generic;

namespace PlayTogether.Models
{
    public partial class GamesLog
    {
        public int ModificationId { get; set; }
        public string Modifier { get; set; }
        public string ModificationType { get; set; }
        public int? OldGameId { get; set; }
        public int? NewGameId { get; set; }
        public int? OldHostUser { get; set; }
        public int? NewHostUser { get; set; }
        public DateTime? OldGameDate { get; set; }
        public DateTime? NewGameDate { get; set; }
        public TimeSpan? OldGameLength { get; set; }
        public TimeSpan? NewGameLength { get; set; }
        public sbyte? OldGameType { get; set; }
        public sbyte? NewGameType { get; set; }
        public int? OldMaxPlayers { get; set; }
        public int? NewMaxPlayers { get; set; }
        public sbyte? OldPrice { get; set; }
        public sbyte? NewPrice { get; set; }
        public int? OldPlaceId { get; set; }
        public int? NewPlaceId { get; set; }
        public string OldGameStatus { get; set; }
        public string NewGameStatus { get; set; }
        public string OldNotes { get; set; }
        public string NewNotes { get; set; }
        public DateTime ModificationTime { get; set; }
    }
}
