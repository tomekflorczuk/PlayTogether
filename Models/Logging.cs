using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging.Abstractions;

namespace PlayTogether.Models
{
    public partial class Logging
    {
        public int User_Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

}
