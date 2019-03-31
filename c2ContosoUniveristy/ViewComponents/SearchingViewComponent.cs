using Microsoft.AspNetCore.Mvc;

namespace RazzorPagesExample.ViewComponents
{
    public class SearchingViewComponent : ViewComponent
    {
       
        public  IViewComponentResult Invoke(bool complate)
        {
            return View(complate);
        }

    }
}
