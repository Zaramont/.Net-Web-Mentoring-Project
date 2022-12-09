using Microsoft.AspNetCore.Mvc;
using MyCatalogSite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCatalogSite.Components
{
    public class BreadCrumbs : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string controller = RouteData.Values["controller"].ToString();
            string action = RouteData.Values["action"].ToString();

            var list = new List<BreadCrumbsPart>();

            if (controller.Equals("Home")) return View(list);
            list.Add(new BreadCrumbsPart { Text = "Home", RelativePath = "/" });

            if (action.Equals("Index"))
            {
                list.Add(new BreadCrumbsPart { Text = controller, RelativePath = $"/{controller}", IsLastPart = true });
            }
            else
            {
                list.Add(new BreadCrumbsPart { Text = controller, RelativePath = $"/{controller}" });
                list.Add(new BreadCrumbsPart { Text = action, IsLastPart = true });
            }

            return View(list);
        }
    }
}
