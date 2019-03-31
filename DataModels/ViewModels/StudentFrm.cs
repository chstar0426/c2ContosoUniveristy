using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class StudentFrm
    {
        public Student Student { get; set; }
        public IList<EnrolledCourseData> enrolledCourseDatas { get; set; }
        public string ErrorMessage { get; set; }
        public FrmType frmType { get; set; }
        
    }
}
