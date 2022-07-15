using System.Collections.Generic;
using WebServer.HTTP;

namespace App.MvcFramework
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route> routeTable);
    }
}