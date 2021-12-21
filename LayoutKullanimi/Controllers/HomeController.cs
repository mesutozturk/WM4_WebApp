using System.Collections.Generic;
using LayoutKullanimi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LayoutKullanimi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new List<Urun>()
            {
                new Urun()
                {
                    Id = 1,
                    Ad = "Elma",
                    Fiyat = 5
                },
                new Urun()
                {
                    Id = 2,
                    Ad = "Armut",
                    Fiyat = 7
                }
            };
            ViewBag.Title = "Ana Sayfa";
            return View(model);
        }
    }
}
