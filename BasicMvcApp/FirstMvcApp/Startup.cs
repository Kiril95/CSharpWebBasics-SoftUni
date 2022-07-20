using App.MvcFramework;
using System.Collections.Generic;
using WebServer.HTTP;

namespace FirstMvcApp
{
    public class Startup : IMvcApplication
    {
        public void Configure(List<Route> routeTable)
        {
            // Empty route
            //routeTable.Add(new Route("/", HttpMethod.Get, (action) =>
            //{
            //    string indexHtml = System.IO.File.ReadAllText("Views/Home/Index.cshtml");
            //    byte[] responseBodyBytes = Encoding.UTF8.GetBytes(indexHtml);

            //    return new HttpResponse("text/html", responseBodyBytes);
            //}));
        }

        public void ConfigureServices()
        {
        }
    }
}