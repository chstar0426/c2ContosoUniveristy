using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace c2ContosoUniveristy.Pages
{
    public class AboutModel : PageModel
    {
        private readonly dbContext _context;

        public AboutModel(dbContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> Students { get; set; }
        

        public async Task OnGetAsync()
        {
            //IQueryable<EnrollmentDateGroup> data =
            //     from student in _context.Students
            //     group student by student.EnrollmentDate into dateGroup
            //     select new EnrollmentDateGroup()
            //     {
            //         EnrollmentDate = dateGroup.Key,
            //         StudentCount = dateGroup.Count()
            //     };

            IQueryable<EnrollmentDateGroup> data =
                _context.Students.GroupBy(s => s.EnrollmentDate)
                .Select(s => new EnrollmentDateGroup
                {
                    EnrollmentDate = s.Key,
                    StudentCount = s.Count()
                });

            Students = await data.AsNoTracking().ToListAsync();

        }
    }
}
