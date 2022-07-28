using FirstMvcApp.Models;
using FirstMvcApp.ViewModels;
using System.Collections.Generic;

namespace FirstMvcApp.Services
{
    public interface ICardService
    {
        void Create(AddCardInputModel input, string userId);

        ICollection<CardViewModel> GetAll();

        ICollection<CardViewModel> UserCards(string userId);

        void RemoveFromCollection(string cardId, string userId);

        void AddToCollection(string cardId, string userId);

        Card GetCard(string cardId);

        void SaveChanges(AddCardInputModel input, string cardId);
    }
}
