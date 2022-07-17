using FirstMvcApp.ViewModels;

namespace FirstMvcApp.Services
{
    public interface IUserService
    {
        public string Create(RegisterInputModel input);

        public string GetUserId(string username, string password);
    }
}
