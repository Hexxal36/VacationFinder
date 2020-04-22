namespace VacationFinder.Web.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;

        public HomeControllerTests(WebApplicationFactory<Startup> server)
        {
            this.server = server;
        }

        [Fact]
        public async Task IndexAsyncShouldReturnCorrectView()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var slogan = "Find the vacation, you always wanted with the help of the VacationFinder";
            Assert.Contains(slogan, responseContent);
        }

        [Fact]
        public async Task PrivacyShouldReturnCorrectView()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Home/Privacy");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var title = "PRIVACY POLICY";
            Assert.Contains(title, responseContent);
        }
    }
}
