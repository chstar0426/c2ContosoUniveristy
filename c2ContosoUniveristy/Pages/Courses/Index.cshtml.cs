using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c2ContosoUniveristy.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; }

        public async Task OnGetAsync()
        {
            Course = await _context.Courses
                .Include(c => c.Department)
                    .ThenInclude(d => d.Administrator)
                .Include(c=>c.CourseAssignments)
                    .ThenInclude(c=>c.Instructor)
                .Include(c=>c.Enrollments)
                    .ThenInclude(e=>e.Student)
                .AsNoTracking().ToListAsync();

        }
    }
}
