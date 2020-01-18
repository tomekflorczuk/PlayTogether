using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlayTogether.Data;
using PlayTogether.Models;

namespace PlayTogether.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly PlayTogether.Data.PtContext _context;

        public CreateModel(PlayTogether.Data.PtContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PlayerId"] = new SelectList(_context.Set<Players>(), "PlayerId", "PlayerId");
        return Page();
        }

        [BindProperty]
        public Models.Users Users { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(Users);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
