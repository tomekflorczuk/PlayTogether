using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PlayTogether.Models
{
    public partial class ProfilePicture
    {
        public IFormFile PictureFile { get; set; }
        public int UserId { get; set; }
    }
}
