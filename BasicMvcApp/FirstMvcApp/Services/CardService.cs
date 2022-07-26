using FirstMvcApp.Data;
using FirstMvcApp.Models;
using FirstMvcApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            StringBuilder errorBuilder = new StringBuilder();

            if (Regex.IsMatch(input.Name, @"\p{IsCyrillic}"))
            {
                throw new ArgumentException("Name must be in lattin letters.");
            }
            if (string.IsNullOrWhiteSpace(input.Name) || input.Name.Length < 5 || input.Name.Length > 30)
            {
                errorBuilder.AppendLine("Name must be between 5 and 30 characters long.<br>");
            };
            if (string.IsNullOrWhiteSpace(input.Image))
            {
                errorBuilder.AppendLine("Image Url cannot be empty.<br>");
            };
            if (string.IsNullOrWhiteSpace(input.Keyword))
            {
                errorBuilder.AppendLine("You must chose a Keyword from the menu.<br>");
            };
            if (!int.TryParse(input.Attack, out _) || int.Parse(input.Attack) < 0)
            {
                errorBuilder.AppendLine("Attack must be a Non-Negative integer.<br>");
            };
            if (!int.TryParse(input.Health, out _) || int.Parse(input.Health) < 0)
            {
                errorBuilder.AppendLine("Health must be a Non-Negative integer.<br>");
            };
            if (string.IsNullOrWhiteSpace(input.Description) || input.Description.Length > 200)
            {
                errorBuilder.AppendLine("Description length cannot be empty or greater than 200 characters.<br>");
            };
            if (input.Description.Contains(@""""))
            {
                errorBuilder.AppendLine("Description field cannot contain double quotes.<br>");
            }

            if (errorBuilder.Length > 0)
            {
                throw new ArgumentException(errorBuilder.ToString().TrimEnd());
            }

            var card = new Card
            {
                Name = input.Name,
                ImageUrl = input.Image,
                Keyword = input.Keyword,
                Attack = int.Parse(input.Attack),
                Health = int.Parse(input.Health),
                Description = input.Description,
            };

            db.Cards.Add(card);
            db.UserCards.Add(new UserCard { UserId = userId, CardId = card.Id });

            db.SaveChanges();
        }

        public ICollection<CardViewModel> GetAll()
        {
            return db.Cards.Select(x => new CardViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Keyword = x.Keyword,
                Attack = x.Attack,
                Health = x.Health,
                Description = x.Description
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
                .OrderBy(x => x.Keyword)
                .ToList();
        }

        public void RemoveFromCollection(string cardId, string userId)
        {
            // Finds the User's card that has both composite keys
            var userCard = db.UserCards.Find(userId, cardId);
            db.UserCards.Remove(userCard);

            db.SaveChanges();
        }

        public void AddToCollection(string cardId, string userId)
        {
            if (db.UserCards.Any(x => x.CardId == cardId && x.UserId == userId))
            {
                throw new ArgumentException("This card is already in your collection.");
            }

            db.UserCards.Add(new UserCard { CardId = cardId, UserId = userId });

            db.SaveChanges();
        }

        public Card GetCard(string cardId)
        {
            return db.Cards.FirstOrDefault(x => x.Id == cardId);
        }

        public void SaveChanges(CardViewModel input, string cardId)
        {
            var targetCard = this.GetCard(cardId);

            targetCard.Name = input.Name;
            targetCard.ImageUrl = input.ImageUrl;
            targetCard.Keyword = input.Keyword;
            targetCard.Attack = input.Attack;
            targetCard.Health = input.Health;
            targetCard.Description = input.Description;

            db.SaveChanges();
        }
    }
}
