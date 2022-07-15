using App.MvcFramework;
using System;
using System.Threading.Tasks;

namespace FirstMvcApp
{
    public class Program
    {
        public static async Task Main()
        {
            await Host.CreateHostAsync(new Startup(), 80);
        }
    }
}
