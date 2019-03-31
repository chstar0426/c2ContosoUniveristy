using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c2ContosoUniveristy.Pages.Students
{
    public class EditModel : StudentEnrollmentPageModel
    {
        private readonly DataModels.dbContext _context;

        public EditModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public StudentFrm studentFrm { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {

            
            if (id == null)
            {
                return NotFound();
            }

            //Student = await _context.Students.FindAsync(id);
            Student = await _context.Students
                    .Include(s => s.Enrollments)
                        .ThenInclude(e => e.Course)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            PopulateEnrolledCourseData(_context, Student);

            studentFrm = new StudentFrm();
            studentFrm.Student = Student;
            studentFrm.ErrorMessage = string.Empty;
            studentFrm.frmType = FrmType.Modify;
            studentFrm.enrolledCourseDatas = EnrolledCourseDataList;

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            
            var formSecurity = Request.Form["SecurityText"].ToString();
            var sessionSecurity = HttpContext.Session.GetString("SecurityText");

            
            if (formSecurity != sessionSecurity)
            {
                studentFrm = new StudentFrm();
                studentFrm.ErrorMessage = "(시크릿트 문자를 확인하세요)";
                studentFrm.frmType = FrmType.Modify;
                
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studenntUpdate = await _context.Students.FindAsync(id);
            
            if (await TryUpdateModelAsync<Student>(
                studenntUpdate, "student",
                s=>s.FirstMidName, s=>s.LastName, s=>s.EnrollmentDate))
            {
                await _context.SaveChangesAsync();

                //패스를 이용하여 Return 관련 변수 화인
                return Redirect("../Index" + System.Web.HttpUtility.UrlDecode(Request.Query["Path"].ToString()));
                //return RedirectToPage("./Index");


            }

            return Page();

        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
