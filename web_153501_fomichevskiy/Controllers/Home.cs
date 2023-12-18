using Microsoft.AspNetCore.Mvc;
using web_153501_fomichevskiy.Models;

namespace web_153501_fomichevskiy.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {

            ViewData["LabNum"] = "Лабораторная работа №2";

			ViewData["List"] = new List<ListDemo>()
            {
                new ListDemo {Id = 1, Name = "1"},
                new ListDemo {Id = 2, Name = "2"}
            };

            return View();
        }
    }
}
