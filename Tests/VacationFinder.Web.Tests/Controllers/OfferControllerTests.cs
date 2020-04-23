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
    using VacationFinder.Data.Models.Enums;
    using VacationFinder.Data.Repositories;
    using VacationFinder.Services.Data;
    using Xunit;

    public class OfferControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> server;
        private readonly ApplicationDbContext context;

        public OfferControllerTests(
            WebApplicationFactory<Startup> server)
        {
            this.server = server;

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

            if (this.context.Tags.Count() == 0)
            {
                this.context.Tags.Add(new Tag()
                {
                    Title = "TestingTag",
                    Sort = 1,
                    IsActive = true,
                });
            }

            if (this.context.Transports.Count() == 0)
            {
                this.context.Transports.Add(new Transport()
                {
                    Sort = 1,
                    Title = "TestingTransport",
                    IsActive = true,
                });
            }

            if (this.context.Offers.Count() == 0)
            {
                this.context.Add(new Offer()
                {
                    Title = "TestingOffer",
                    Days = 1,
                    Nights = 1,
                    Places = 1,
                    Price = 1,
                    TagId = this.context.Tags.First().Id,
                    HotelId = this.context.Hotels.First().Id,
                    TransportId = this.context.Transports.First().Id,
                    Description = "test",
                    IsActive = true,
                });
            }

            this.context.SaveChanges();
        }

        [Fact]
        public async Task DetailsShouldReturnCorrectPageIfOfferExists()
        {
            var offer = this.context.Offers.First();

            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Offer/Details/" + offer.Id);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(offer.Price.ToString(), responseContent);
        }

        [Fact]
        public async Task DetailsShouldRedirectToTheIndexPageIfOfferIdIsInvalid()
        {
            var client = this.server.CreateClient();
            var response = await client.GetAsync("/Offer/Details/-10");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
