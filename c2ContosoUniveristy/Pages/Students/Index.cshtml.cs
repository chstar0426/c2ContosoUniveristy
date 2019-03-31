using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModels;

namespace c2ContosoUniveristy.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly DataModels.dbContext _context;

        public IndexModel(DataModels.dbContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get;set; }

        #region 페이징관련 변수
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public bool SearchMode { get; set; } = false;
        #endregion


        #region 정렬관련 변수
        public string NameSort { get; set; }
        public string DateSort { get; set; } 
        #endregion



        public async Task OnGetAsync()
        {

            string sortOrder = string.Empty;

            int pageIndex = 0;
            int pageSize = 3;

            string searchField = string.Empty;
            string searchQuery = string.Empty;

            /////////////////////////////////////////////////////////////////////////

            if (!String.IsNullOrEmpty(Request.Query["SortOrder"]))
            {
                sortOrder = Request.Query["SortOrder"];

            }

            NameSort = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : "";
            DateSort = sortOrder == "Date" ? "Date_Desc" : "Date";


            ////////////////////////////////////////////////////////////////////////////////////

            if (!String.IsNullOrEmpty(Request.Query["SearchField"]) &&
                 !String.IsNullOrEmpty(Request.Query["SearchQuery"]))
            {
                SearchMode = true;
                searchField = Request.Query["SearchField"];
                searchQuery = Request.Query["SearchQuery"];
            }

            /////////////////////////////////////////////////////////////////////////////////


            //var iStudent2 = from s in _context.Students select s;

            var iStudent = (IQueryable<Student>)_context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course);

            if (SearchMode)
            {
                switch (searchField)
                {
                    case "Name":
                        iStudent = iStudent.Where(s => s.FirstMidName.Contains(searchQuery) || s.LastName.Contains(searchQuery));
                        break;

                    default:
                        break;
                }

            }
            

            switch (sortOrder)
            {
                case "Name_Desc":
                    iStudent = iStudent.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    iStudent = iStudent.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date_Desc":
                    iStudent = iStudent.OrderByDescending(s => s.EnrollmentDate);
                    break;

                default:
                    iStudent = iStudent.OrderBy(s => s.LastName);
                    break;
            }
            

            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"]))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
                //Response.Cookies.Append("Page", pageIndex.ToString());
            }

            TotalCount = iStudent.Count();
            PageIndex = pageIndex + 1;

            Student = await iStudent
                .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();
            
            
            
        }
    }
}
