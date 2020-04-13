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

    public class CityController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        private readonly IPagingService pagingService;

        public CityController(ApplicationDbContext context)
        {
            this._context = context;
            this.pagingService = new PagingService();
        }

        public async Task<IActionResult> Index(int? page, int? perPage, string? country, string? name, string? order)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this._context.Cities.ToListAsync();

            if (order != null)
            {
                switch (order)
                {
                    case "name":
                        list = list.OrderBy(t => t.Name).ToList();
                        break;
                    case "country":
                        list = list.OrderBy(t => t.Country.Name).ToList();
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
                if (country != null)
                {
                    list = list.Where(t => t.CountryId == int.Parse(country)).ToList();
                }
            }
            catch { }

            this.ViewBag.Pages = this.pagingService.GetPageCount(list, pageSize);
            this.ViewBag.Countries = await this._context.Countries.ToListAsync();

            return this.View(this.pagingService.GetPage(list, pageNumber, pageSize).Cast<City>().ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var city = await this._context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return this.NotFound();
            }

            return this.View(city);
        }

        // GET: Admin/City/Create
        public async Task<IActionResult> Create()
        {
            this.ViewBag.Countries = await this._context.Countries.ToListAsync();

            return this.View();
        }

        // POST: Admin/City/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CountryId,Id,")] City city)
        {
            city.CreatedOn = DateTime.Now.AddHours(-3);
            city.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this._context.Add(city);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(city);
        }

        // GET: Admin/City/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var City = await this._context.Cities.FindAsync(id);
            if (City == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Countries = await this._context.Countries.ToListAsync();

            return this.View(City);
        }

        // POST: Admin/City/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,CountryId,Id,")] City city)
        {
            if (id != city.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(city);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CityExists(city.Id))
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

            return this.View(city);
        }

        // GET: Admin/City/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var City = await this._context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (City == null)
            {
                return this.NotFound();
            }

            return this.View(City);
        }

        // POST: Admin/City/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var City = await this._context.Cities.FindAsync(id);
            City.IsDeleted = true;
            City.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CityExists(int id)
        {
            return this._context.Cities.Any(e => e.Id == id);
        }
    }
}
