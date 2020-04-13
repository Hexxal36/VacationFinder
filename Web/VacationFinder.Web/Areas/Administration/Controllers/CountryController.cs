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

    public class CountryController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        public CountryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Countries.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var country = await this.context.Countries
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
                this.context.Add(country);
                await this.context.SaveChangesAsync();
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

            var country = await this.context.Countries.FindAsync(id);
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
                    this.context.Update(country);
                    await this.context.SaveChangesAsync();
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

            var country = await this.context.Countries
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
            var country = await this.context.Countries.FindAsync(id);
            country.IsDeleted = true;
            country.DeletedOn = DateTime.Now.AddHours(-3);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CountryExists(int id)
        {
            return this.context.Countries.Any(e => e.Id == id);
        }
    }
}
