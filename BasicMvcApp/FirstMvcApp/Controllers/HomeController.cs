using App.MvcFramework;
using FirstMvcApp.ViewModels;
using System;
using WebServer.HTTP;

namespace FirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        //[HttpGet("/")]
        public HttpResponse Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.CurrentYear = DateTime.UtcNow.Year;
            viewModel.Message = "Welcome to Battle Cards";

            if (this.Request.Session.ContainsKey("AboutPage"))
            {
                viewModel.SpecialMessage = "WE WERE ON THE ABOUT PAGE! :]";
            }

            return this.View(viewModel);
        }

        // GET /home/about
        public HttpResponse About()
        {
            this.Request.Session["AboutPage"] = "test";
            return this.View();
        }
    }
}
