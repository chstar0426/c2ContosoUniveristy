using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace c2ContosoUniveristy.ViewComponents
{
    public class InstructorIndexViewComponent : ViewComponent
    {
        private readonly DataModels.dbContext _context;

        public InstructorIndexViewComponent(DataModels.dbContext context)
        {
            _context = context;
        }



        public IViewComponentResult Invoke(int courseID)
        {
            var selectedCourse = _context.Courses
                .Include(c => c.Enrollments)
                    .ThenInclude(c => c.Student)
                .Where(c => c.CourseID == courseID).Single();

            return View(selectedCourse);

        }

    }
}
