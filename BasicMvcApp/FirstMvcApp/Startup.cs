using App.MvcFramework;
using FirstMvcApp.Data;
using FirstMvcApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebServer.HTTP;

namespace FirstMvcApp
{
    public class Startup : IMvcApplication
    {
        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUserService, UserService>();
            serviceCollection.Add<ICardService, CardService>();
        }
    }
}