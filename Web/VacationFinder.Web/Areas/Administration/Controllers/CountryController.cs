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
    using VacationFinder.Data.Models.Enums;
    using VacationFinder.Web.ViewModels.Administration.Country;

    public class CountryController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public CountryController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index(int? page, int? perPage, string? order, string? name, string? continent)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this._context.Countries.ToListAsync();

            if (order != null)
            {
                switch (order)
                {
                    case "name":
                        list = list.OrderBy(t => t.Name).ToList();
                        break;
                    case "continent":
                        list = list.OrderBy(t => t.Continent).ToList();
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

            if (continent != null)
            {
                list = list.
                    Where(t => 
                    t.Continent == (Continent)int.Parse(continent.Substring(0, 1)))
                    .ToList();
            }

            return this.View(new IndexViewModel() { List = GetPage(list, pageNumber, pageSize), Pages = GetPageCount(list, pageSize) });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var country = await this._context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return this.NotFound();
            }

            return this.View(country);
        }

        // GET: Admin/Country/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Admin/Country/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Continent,Id")] Country country)
        {
            country.CreatedOn = DateTime.Now.AddHours(-3);
            country.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this._context.Add(country);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }
            return this.View(country);
        }

        // GET: Admin/Country/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var country = await this._context.Countries.FindAsync(id);
            if (country == null)
            {
                return this.NotFound();
            }
            return this.View(country);
        }

        // POST: Admin/Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Continent,Id")] Country country)
        {


            if (id != country.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(country);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CountryExists(country.Id))
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

            return this.View(country);
        }

        // GET: Admin/Country/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var country = await this._context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return this.NotFound();
            }

            return this.View(country);
        }

        // POST: Admin/Country/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await this._context.Countries.FindAsync(id);
            country.IsDeleted = true;
            country.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CountryExists(int id)
        {
            return this._context.Countries.Any(e => e.Id == id);
        }

        private static IEnumerable<Country> GetPage(IEnumerable<Country> list, int pageNumber, int pageSize = 10)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        private static int GetPageCount(IEnumerable<Country> list, int pageSize = 10)
        {
            int count = list.Count();

            if (count == 0)
            {
                return 1;
            }

            if (count % pageSize == 0)
            {
                return count / pageSize;
            }

            return (count / pageSize) + 1;
        }


    }
}
