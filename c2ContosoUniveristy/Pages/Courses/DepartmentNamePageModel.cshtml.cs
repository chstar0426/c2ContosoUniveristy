﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace c2ContosoUniveristy.Pages.Courses
{
    public class DepartmentNamePageModel: PageModel
    {

        public SelectList DepartmentNameSL { get; set; }

        public void PopulateDepartmentsDropdownList(dbContext _context, object selectDepartment =null)
        {
            var departmentsQuery = from d in _context.Departments orderby d.Name select d;

            DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(), "DepartmentID", "Name", selectDepartment);
                                 
        }
     }
}