using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;

namespace c2ContosoUniveristy.Pages.Instructors
{
    public class CreateModel : InstructorCoursedPageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var newInstructor = new Instructor();
            newInstructor.CourseAssignments = new List<CourseAssignment>();

            PopulateAssignedCourseData(_context, newInstructor);

            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newInstructor = new Instructor();

            if (selectedCourses!=null)
            {
                newInstructor.CourseAssignments = new List<CourseAssignment>();

                foreach (var item in selectedCourses)
                {
                    newInstructor.CourseAssignments.Add(new CourseAssignment
                    {
                        CourseID = int.Parse(item)
                    });

                }
            }

            if (await TryUpdateModelAsync<Instructor>(newInstructor, "instructor",
                i=>i.FirstMidName, i=>i.LastName, i=>i.HireDate, i=>i.OfficeAssignment))
            {
                _context.Instructors.Add(newInstructor);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
                
            }

            PopulateAssignedCourseData(_context, newInstructor);
            return Page();
            
        }
    }
}