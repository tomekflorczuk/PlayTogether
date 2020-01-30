using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayTogether.Models;

namespace PlayTogether.Data
{
    public class AppData
    {
        public int LoggedId { get; set; }
        public int SelectedSportType { get; set; } = 0;
        public int SelectedCity { get; set; } = 1;
        public Places SelectedPlace { get; set; }
        public string BucketName { get; set; }
    }
}
