using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace c2ContosoUniveristy.Pages.Students
{
    public class StudentEnrollmentPageModel : PageModel
    {
        public IList<EnrolledCourseData> EnrolledCourseDataList { get; set; }

        public void PopulateEnrolledCourseData(dbContext _context, Student student)
        {

            var studentCourses = new HashSet<int>(student.Enrollments.Select(e => e.CourseID));
            EnrolledCourseDataList = new List<EnrolledCourseData>();

            foreach ( var item in _context.Courses)
            {

                EnrolledCourseDataList.Add (new EnrolledCourseData {
                    CourseID = item.CourseID,
                    Title = item.Title,
                    Enrolled = studentCourses.Contains(item.CourseID)
                });

            }

        }
    }
}