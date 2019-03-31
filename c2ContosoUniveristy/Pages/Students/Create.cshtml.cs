using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModels;
using Microsoft.AspNetCore.Http;

namespace c2ContosoUniveristy.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public CreateModel(DataModels.dbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public StudentFrm studentFrm { get; set; }
        
        public IActionResult OnGet()
        {

            studentFrm = new StudentFrm();
            studentFrm.ErrorMessage = string.Empty;
            studentFrm.frmType = FrmType.Write;
            
            return Page();
        }

        


        public async Task<IActionResult> OnPostAsync()
        {

            var formSecurity = Request.Form["SecurityText"].ToString();
            var sessionSecurity = HttpContext.Session.GetString("SecurityText");

            if (formSecurity != sessionSecurity)
            {

                studentFrm = new StudentFrm();
                studentFrm.ErrorMessage = "(시크릿트 문자를 확인하세요)";
                studentFrm.frmType = FrmType.Write;
                
                return Page();
            }


            if (!ModelState.IsValid)
            {
                return Page();
            }


            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent, "student",
                s=>s.FirstMidName, s=>s.LastName, s=>s.EnrollmentDate))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
                
            }

            return Page();

        }
    }
}