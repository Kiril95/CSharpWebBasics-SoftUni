using App.MvcFramework;
using FirstMvcApp.Models;
using FirstMvcApp.Services;
using FirstMvcApp.ViewModels;
using System;
using WebServer.HTTP;

namespace FirstMvcApp.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardService cardService;

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
            cardService.RemoveFromCollection(cardId, GetUserId());

            return Redirect("/Cards/Collection");
        }

        public HttpResponse AddToCollection(string cardId)
        {
            try
            {
                cardService.AddToCollection(cardId, GetUserId());

                return Redirect("/Cards/Collection");
            }
            catch (ArgumentException ae)
            {
                return Error(ae.Message);
            }
        }

        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return Redirect("/Users/Login");
            }

            return View();
        }

        [HttpPost]
        public HttpResponse Add(AddCardInputModel input)
        {
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

        [HttpGet]
        public HttpResponse Edit(string cardId)
        {
            var targetCard = cardService.GetCard(cardId);

            return View(targetCard);
        }

        [HttpPost]
        public HttpResponse Edit(CardViewModel input, string cardId)
        {
            try
            {
                cardService.SaveChanges(input, cardId);

                return Redirect("/Cards/Collection");
            }
            catch (ArgumentException ae)
            {
                return Error(ae.Message);
            }
        }
    }
}
