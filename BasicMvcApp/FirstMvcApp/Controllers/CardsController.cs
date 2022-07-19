using App.MvcFramework;
using FirstMvcApp.Services;
using FirstMvcApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.HTTP;

namespace FirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardService cardService;

        public CardsController()
        {
        }

        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        public HttpResponse All()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var allCards = cardService.GetAll();

            return View(allCards);
        }

        public HttpResponse Collection()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            var userCards = cardService.UserCards(GetUserId());

            return View(userCards);
        }

        public HttpResponse RemoveFromCollection(string cardId)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            cardService.RemoveFromCollection(cardId, GetUserId());

            return Redirect("/Cards/Collection");
        }

        public HttpResponse AddToCollection(string cardId)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            cardService.AddToCollection(cardId, GetUserId());

            return Redirect("/Cards/Collection");
        }

        public HttpResponse Add()
        {
            if (IsUserSignedIn())
            {
                return View();
            }

            return Redirect("/Users/Login");
        }

        [HttpPost]
        public HttpResponse Add(AddCardInputModel input)
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            try
            {
                cardService.Create(input, GetUserId());
                return Redirect("/Cards/All");
            }
            catch (ArgumentException ae)
            {
                return Error(ae.Message);
            }
        }

    }
}
