using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(VacationFinder.Web.Areas.Identity.IdentityHostingStartup))]

namespace VacationFinder.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
