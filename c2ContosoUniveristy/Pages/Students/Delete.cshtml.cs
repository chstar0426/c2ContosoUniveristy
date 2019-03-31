using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c2ContosoUniveristy.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public DeleteModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }
        public string  ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            if (saveChangeError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try agin";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s=>s.ID==id);


            if (Student==null)
            {
                return NotFound();
            }

            try
            {
                _context.Students.Remove(Student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

            }
            catch (Exception)
            {
                return RedirectToPage("./Delete", new { id, saveChangeError = true });
                
            }
           
        }
    }
}
