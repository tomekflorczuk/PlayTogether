using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Data
{
    public class AppData
    {
        public int LoggedId { get; set; }
        public int SelectedSportType { get; set; }
        public int SelectedCity { get; set; }
        public string BucketName { get; set; }
        public string PictureUrl { get; set; }
    }
}
