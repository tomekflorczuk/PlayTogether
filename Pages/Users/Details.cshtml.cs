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
    public class DetailsModel : PageModel
    {
        private readonly PlayTogether.Data.PtContext _context;

        public DetailsModel(PlayTogether.Data.PtContext context)
        {
            _context = context;
        }

        public Models.Users Users { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Users = await _context.Users
                .Include(u => u.Player)
                .Include(u => u.Role).FirstOrDefaultAsync(m => m.UserId == id);

            if (Users == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
