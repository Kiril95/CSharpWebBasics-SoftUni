using FirstMvcApp.Data;
using FirstMvcApp.Models;
using FirstMvcApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace FirstMvcApp.Services
{
    class CardService : ICardService
    {
        private ApplicationDbContext db;

        public CardService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(AddCardInputModel input, string userId)
        {
            var card = new Card
            {
                Name = input.Name,
                Attack = int.Parse(input.Attack),
                Health = int.Parse(input.Health),
                Keyword = input.Keyword,
                Description = input.Description,
                ImageUrl = input.Image
            };

            db.Cards.Add(card);
            db.UserCards.Add(new UserCard { UserId = userId, CardId = card.Id });

            db.SaveChanges();
        }

        public ICollection<CardViewModel> GetAll()
        {
            return db.Cards.Select(x => new CardViewModel
            {
                Attack = x.Attack,
                Health = x.Health,
                ImageUrl = x.ImageUrl,
                Description = x.Description,
                Id = x.Id,
                Keyword = x.Keyword,
            })
            .ToList();
        }

        public ICollection<CardViewModel> UserCards(string userId)
        {
            return db.UserCards
                .Where(c => c.UserId == userId)
                .Select(x => new CardViewModel
                {
                    Name = x.Card.Name,
                    Attack = x.Card.Attack,
                    Health = x.Card.Health,
                    ImageUrl = x.Card.ImageUrl,
                    Description = x.Card.Description,
                    Id = x.Card.Id,
                    Keyword = x.Card.Keyword,
                })
                .ToList();
        }

        public void RemoveFromCollection(string cardId, string userId)
        {
            var userCard = db.UserCards.Find(userId, cardId);
            db.UserCards.Remove(userCard);

            db.SaveChanges();
        }

        public void AddToCollection(string cardId, string userId)
        {
            if (!db.UserCards.Any(x => x.CardId == cardId && x.UserId == userId))
            {
                db.UserCards.Add(new UserCard { CardId = cardId, UserId = userId });
                db.SaveChanges();
            }
        }

    }
}
