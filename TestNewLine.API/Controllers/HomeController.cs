﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestNewLine.API.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
