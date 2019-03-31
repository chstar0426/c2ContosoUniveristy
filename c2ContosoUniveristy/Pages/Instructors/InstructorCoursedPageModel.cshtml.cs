using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace c2ContosoUniveristy.Pages.Instructors
{
    public class InstructorCoursedPageModel : PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList ;

        public void PopulateAssignedCourseData(dbContext _context, Instructor instructor)
        {
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            AssignedCourseDataList = new List<AssignedCourseData>();

            foreach (var item in _context.Courses)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                     CourseID=item.CourseID,
                     Title=item.Title,
                     Assigned = instructorCourses.Contains(item.CourseID)
                });

            }

        }

        public void UpdateInstructorCourse(dbContext _context,string[] selectedCourses, Instructor insturctorToUpdate)
        {
            if (selectedCourses == null)
            {
                insturctorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var InstructorCoursesHS = new HashSet<int>(insturctorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));

            foreach (var item in _context.Courses)
            {
                if (selectedCoursesHS.Contains(item.CourseID.ToString()))
                {
                    if (!InstructorCoursesHS.Contains(item.CourseID))
                    {
                        insturctorToUpdate.CourseAssignments.Add(
                            new CourseAssignment
                            {
                                InstructorID = insturctorToUpdate.ID,
                                CourseID = item.CourseID

                            });


                    }
                }
                else
                {
                    if(InstructorCoursesHS.Contains(item.CourseID))
                    {
                        CourseAssignment courseToRemove
                            = insturctorToUpdate.CourseAssignments.SingleOrDefault(i => i.CourseID == item.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }

            }
        }
    }
}