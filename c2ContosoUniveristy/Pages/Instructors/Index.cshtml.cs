using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c2ContosoUniveristy.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public InstructorIndexData Instructor { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }


        public async Task OnGetAsync(int? id, int? courseID)
        {
            Instructor = new InstructorIndexData();

            Instructor.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                //    .ThenInclude(c => c.Course)
                //        .ThenInclude(i=>i.Department)
                //.Include(i=>i.CourseAssignments)
                //    .ThenInclude(i=>i.Course)
                //        .ThenInclude(i=>i.Enrollments)
                //            .ThenInclude(i=>i.Student)
                //.AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();


            if (id != null)
            {
                InstructorID = id.Value;

                var seletedInstructor = Instructor.Instructors.Where(i => i.ID == id).Single();
                
                //1. Load를 이용한 별도 쿼리
                //await _context.CourseAssignments.Where(c => c.InstructorID == seletedInstructor.ID).LoadAsync();

                //foreach (var item in seletedInstructor.CourseAssignments)
                //{
                //    await _context.Courses.Include(c => c.Department).Where(c => c.CourseID == item.CourseID).LoadAsync();
                //}


                //2. 명시적 로드
                await _context.Entry(seletedInstructor).Collection(x => x.CourseAssignments).LoadAsync();

                foreach (var item in seletedInstructor.CourseAssignments)
                {
                    await _context.Entry(item).Reference(x => x.Course).LoadAsync();
                    await _context.Entry(item.Course).Reference(x => x.Department).LoadAsync();


                }

                Instructor.Courses = seletedInstructor.CourseAssignments.Select(c => c.Course);                    
                
            }

            if (courseID != null)
            {
                CourseID = courseID.Value;


                //1. 즉시로드
                //Instructor.Enrollments = Instructor.Courses.Where(c => c.CourseID == courseID).Single().Enrollments;

                //2. 별도로드
                var selectedCourse = Instructor.Courses.Where(x => x.CourseID == courseID).Single();
                await _context.Enrollments.Include(e=>e.Student).Where(e => e.CourseID == selectedCourse.CourseID).LoadAsync();
                
                //foreach (var item in selectedCourse.Enrollments)
                //{
                //    await _context.Students.Where(s => s.ID == item.StudentID).LoadAsync();
                //}
                
                Instructor.Enrollments = selectedCourse.Enrollments;


                //3.명시적 로드
                //var selectedCourse = Instructor.Courses.Where(x => x.CourseID == courseID).Single();
                //await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                //foreach (var enrollment in selectedCourse.Enrollments)
                //{
                //    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();

                //}
                //Instructor.Enrollments = selectedCourse.Enrollments;




            }

        }
    }
}
