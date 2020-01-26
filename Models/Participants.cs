using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Models
{
    [Table("Participants")]
    public partial class Participants
    {
        [Key]
        public int ParticipantId { get; set; }
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public string ParticipantStatus { get; set; }
        public DateTime Added { get; set; }
        public DateTime Modified { get; set; }

        public virtual Games Game { get; set; }
        public virtual Players Player { get; set; }
    }
}
