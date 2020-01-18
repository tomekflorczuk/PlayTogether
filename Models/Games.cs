using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Models
{
    [Table("Games")]
    public partial class Games
    {
        public Games()
        {
            Participants = new HashSet<Participants>();
        }

        [Key]
        public int GameId { get; set; }
        public int HostUser { get; set; }
        public DateTime GameDate { get; set; }
        public TimeSpan GameLength { get; set; }
        public sbyte GameType { get; set; }
        public int MaxPlayers { get; set; }
        public sbyte? Price { get; set; }
        public sbyte PlaceId { get; set; }
        public DateTime Created { get; set; }
        public string GameStatus { get; set; }
        public string Notes { get; set; }
        public DateTime Modified { get; set; }

        [ForeignKey("GameType")]
        public virtual SportTypes GameTypeNavigation { get; set; }
        [ForeignKey("HostUser")]
        public virtual Users HostUserNavigation { get; set; }
        [ForeignKey("PlaceId")]
        public virtual Places Place { get; set; }
        public virtual ICollection<Participants> Participants { get; set; }
    }
}
