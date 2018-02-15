using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace raupjc_projekt.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}