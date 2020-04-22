namespace VacationFinder.Web.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Data.Repositories;
    using VacationFinder.Services.Data;
    using Xunit;

    public class HotelControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;

        public HotelControllerTests(
            WebApplicationFactory<Startup> server)
        {
            this.server = server;
        }

        [Fact]
        public async Task FilteredHotelsShouldAllHaveTheFilteredCharacteristic()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Hotel?name=test&stars=&city=");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var hotelInfoString = "<p class=\"hotel-info\">test";
            var hotelAnchorString = "<a class=\"hotel-details-link\"";
            Assert.Equal(
                Regex.Matches(responseContent, hotelInfoString).Count(),
                Regex.Matches(responseContent, hotelAnchorString).Count());
        }

        [Fact]
        public async Task DetailsShouldRedirectToTheIndexPageIfHotelIdIsInvalid()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Hotel/-10");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
