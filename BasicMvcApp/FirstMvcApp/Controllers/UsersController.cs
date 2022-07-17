using App.MvcFramework;
using FirstMvcApp.Services;
using FirstMvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.HTTP;

namespace FirstMvcApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController()
        {
        }

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public HttpResponse Login()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            var userId = userService.GetUserId(username, password);
            if (string.IsNullOrEmpty(userId))
            {
                return Error("Incorrect username or password");
            }

            SignIn(userId);

            return Redirect("/Cards/All");
        }

        [HttpGet]
        public HttpResponse Register()
        {
            if (IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            if (IsUserSignedIn())
            {
                return Redirect("Cards/All");
            }

            try
            {
                var userId = userService.Create(input);
                this.SignIn(userId);

                return Redirect("/Cards/All");
            }

            catch (ArgumentException e)
            {
                return Error(e.Message);
            }
        }

    }
}
