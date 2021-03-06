﻿namespace VacationFinder.Web.Tests.Controllers
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
    using VacationFinder.Data.Models.Enums;
    using VacationFinder.Data.Repositories;
    using VacationFinder.Services.Data;
    using Xunit;

    public class HotelControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;
        private readonly ApplicationDbContext context;

        public HotelControllerTests(
            WebApplicationFactory<Startup> server)
        {
            this.server = server;
            this.context = new ApplicationDbContext(
               new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlServer("Server=.;Database=VacationFinder;Trusted_Connection=True;MultipleActiveResultSets=true")
               .Options);

            if (this.context.Countries.Count() == 0)
            {
                this.context.Countries.Add(new Country()
                {
                    Name = "TestingCountry",
                    Continent = Continent.Unknown,
                });
            }

            if (this.context.Cities.Count() == 0)
            {
                this.context.Cities.Add(new City()
                {
                    Name = "TestingCity",
                    CountryId = this.context.Countries.First().Id,
                });
            }

            if (this.context.Hotels.Count() == 0)
            {
                this.context.Hotels.Add(new Hotel()
                {
                    Name = "TestingHotel",
                    CityId = this.context.Cities.OrderBy(x => x.Id).First().Id,
                    ImageUrl = "https://media.sproutsocial.com/uploads/2017/02/10x-featured-social-media-image-size.png",
                    Stars = 4,
                    Description = "test",
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        public async Task FilteredHotelsShouldAllHaveTheFilteredCharacteristic()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Hotel?name=&stars=3&city=");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            var hotelStarsString = "<p class=\"hotel-stars\">3";
            var hotelAnchorString = "<a class=\"hotel-details-link\"";
            Assert.Equal(
                Regex.Matches(responseContent, hotelStarsString).Count(),
                Regex.Matches(responseContent, hotelAnchorString).Count());
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectPageIfHotelExists()
        {
            var hotel = this.context.Hotels.First();

            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Hotel/Details/" + hotel.Id);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(hotel.ImageUrl, responseContent);
        }

        [Fact]
        public async Task DetailsShouldReturnNotFoundIfHotelIdIsInvalid()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Hotel/Details/-10");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
