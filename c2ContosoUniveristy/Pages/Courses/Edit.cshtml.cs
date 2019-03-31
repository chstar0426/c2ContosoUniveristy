using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c2ContosoUniveristy.Pages.Courses
{
    public class EditModel : DepartmentNamePageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //현재 코드에서는 Department를 Include할 필요가 없을 것 같음
            //Course = await _context.Courses
            //    .Include(c => c.Department).FirstOrDefaultAsync(m => m.CourseID == id);

            Course = await _context.Courses.FindAsync(id);

            if (Course == null)
            {
                return NotFound();
            }

            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", Course.Department);
            PopulateDepartmentsDropdownList(_context, Course.DepartmentID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Courses.FindAsync(id);


            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await TryUpdateModelAsync<Course>(courseToUpdate, "course", 
                c=>c.Credits, c=>c.DepartmentID, c=>c.Title))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./index");
            }

            PopulateDepartmentsDropdownList(_context, courseToUpdate.DepartmentID);
            return Page();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
