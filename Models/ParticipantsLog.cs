using System;
using System.Collections.Generic;

namespace PlayTogether.Models
{
    public partial class ParticipantsLog
    {
        public int ModificationId { get; set; }
        public string Modifier { get; set; }
        public string ModificationType { get; set; }
        public int? OldParticipantId { get; set; }
        public int? NewParticiapntId { get; set; }
        public int? OldUserId { get; set; }
        public int? NewUserId { get; set; }
        public int? OldGameId { get; set; }
        public int? NewGameId { get; set; }
        public string OldParticipantStatus { get; set; }
        public string NewParticipantStatus { get; set; }
        public DateTime ModificationTime { get; set; }
    }
}
