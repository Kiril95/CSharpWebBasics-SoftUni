using FirstMvcApp.ViewModels;
using System.Collections.Generic;

namespace FirstMvcApp.Services
{
    public interface ICardService
    {
        void Create(AddCardInputModel input);

        ICollection<CardViewModel> GetAll();

        ICollection<CardViewModel> UserCards(string userId);

        void RemoveFromCollection(string cardId, string userId);

        void AddToCollection(string cardId, string userId);
    }
}
