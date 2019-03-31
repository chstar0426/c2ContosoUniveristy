using DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace c2ContosoUniveristy.ViewComponents
{
    public class StudentFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(StudentFrm student)
        {
            return View("Default", student);
        }

    }
}
