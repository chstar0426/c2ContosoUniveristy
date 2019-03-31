using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;

namespace c2ContosoUniveristy.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropdownList(_context);
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyCourse = new Course();

            if (await TryUpdateModelAsync<Course>(emptyCourse,"course",
                c=>c.CourseID, c=>c.DepartmentID, c=>c.Title, c=>c.Credits))
            {
                _context.Courses.Add(Course);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");

            }

            PopulateDepartmentsDropdownList(_context, emptyCourse.DepartmentID);
            return Page();

           
        }
    }
}