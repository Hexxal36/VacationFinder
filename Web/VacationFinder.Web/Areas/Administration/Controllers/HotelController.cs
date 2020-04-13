namespace VacationFinder.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using VacationFinder.Data;
    using VacationFinder.Data.Models;
    using VacationFinder.Services;

    public class HotelController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        private readonly IPagingService pagingService;

        public HotelController(ApplicationDbContext context)
        {
            this.context = context;
            this.pagingService = new PagingService();
        }

#nullable enable

        public async Task<IActionResult> Index(int? page, int? perPage, string? order, string? name, string? city, string? stars)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this.context.Hotels.ToListAsync();

            if (order != null)
            {
                switch (order)
                {
                    case "name":
                        list = list.OrderBy(t => t.Name).ToList();
                        break;
                    case "city":
                        list = list.OrderBy(t => t.City.Name).ToList();
                        break;
                    case "stars":
                        list = list.OrderByDescending(t => t.Stars).ToList();
                        break;
                    case "isActive":
                        list = list.OrderByDescending(t => t.IsActive).ToList();
                        break;
                    case "createdOn":
                        list = list.OrderByDescending(t => t.CreatedOn).ToList();
                        break;
                    case "modifiedOn":
                        list = list.OrderByDescending(t => t.ModifiedOn).ToList();
                        break;
                }
            }

            if (name != null)
            {
                list = list.Where(t => t.Name.Contains(name)).ToList();
            }

            try
            {
                if (city != null)
                {
                    list = list.Where(t => t.City.Id == int.Parse(city)).ToList();
                }

                if (stars != null)
                {
                    list = list.Where(t => t.Stars == int.Parse(stars)).ToList();
                }
            }
            catch
            {
            }

            this.ViewBag.Cities = await this.context.Cities.ToListAsync();

            this.ViewBag.Pages = this.pagingService.GetPageCount(list, pageSize);

            return this.View(this.pagingService.GetPage(list, pageNumber, pageSize).Cast<Hotel>().ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hotel = await this.context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return this.NotFound();
            }

            return this.View(hotel);
        }

        // GET: Admin/Hotel/Create
        public async Task<IActionResult> Create()
        {
            this.ViewBag.Cities = await this.context.Cities.ToListAsync();

            return this.View();
        }

        // POST: Admin/Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Stars,ImageUrl,Id,IsActive,Description,CityId")] Hotel hotel)
        {
            hotel.CreatedOn = DateTime.Now.AddHours(-3);
            hotel.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this.context.Add(hotel);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(hotel);
        }

        // GET: Admin/Hotel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hotel = await this.context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Cities = await this.context.Cities.ToListAsync();

            return this.View(hotel);
        }

        // POST: Admin/Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Stars,ImageUrl,Id,IsActive,Description,CityId")] Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.context.Update(hotel);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.HotelExists(hotel.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(hotel);
        }

        // GET: Admin/Hotel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Hotel hotel = await this.context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return this.NotFound();
            }

            return this.View(hotel);
        }

        // POST: Admin/Hotel/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await this.context.Hotels.FindAsync(id);
            hotel.IsDeleted = true;
            hotel.DeletedOn = DateTime.Now.AddHours(-3);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool HotelExists(int id)
        {
            return this.context.Hotels.Any(e => e.Id == id);
        }
    }
}
