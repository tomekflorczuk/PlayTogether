using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Data;
using PlayTogether.Models;

namespace PlayTogether.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly PlayTogether.Data.PtContext _context;

        public IndexModel(PlayTogether.Data.PtContext context)
        {
            _context = context;
        }

        public IList<Models.Users> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users
                .Include(u => u.Player)
                .Include(u => u.Role).ToListAsync();
        }
    }
}