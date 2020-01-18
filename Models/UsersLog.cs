using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Models
{
    [Table("UsersLog")]
    public partial class UsersLog
    {
        [Key]
        public int ModificationId { get; set; }
        public string Modifier { get; set; }
        public string ModificationType { get; set; }
        public int? OldUserId { get; set; }
        public int? NewUserId { get; set; }
        public string OldLogin { get; set; }
        public string NewLogin { get; set; }
        public sbyte? OldRoleId { get; set; }
        public sbyte? NewRoleId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string OldUserStatus { get; set; }
        public string NewUserStatus { get; set; }
        public int? OldPlayerId { get; set; }
        public int? NewPlayerId { get; set; }
        public DateTime ModificationTime { get; set; }
    }
}
