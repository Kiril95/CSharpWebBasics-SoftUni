using App.MvcFramework;
using FirstMvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.HTTP;

namespace FirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            var viewModel = new IndexViewModel();
            viewModel.CurrentYear = DateTime.UtcNow.Year;
            viewModel.Message = "Welcome to Battle Cards";

            return this.View(viewModel);
        }

        [HttpGet("/Logout")]
        public HttpResponse Logout()
        {
            if (IsUserSignedIn())
            {
                SignOut();
            }

            return this.Redirect("/Home/Index");
        }

        public HttpResponse About()
        {
            return this.View();
        }
    }
}
