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

    [AutoValidateAntiforgeryToken]
    public class CityController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public CityController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.Cities.ToListAsync());
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

            var city = await this._context.Cities.FindAsync(id);
            if (city == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Countries = await this._context.Countries.ToListAsync();

            return this.View(city);
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

            var city = await this._context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return this.NotFound();
            }

            return this.View(city);
        }

        // POST: Admin/City/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await this._context.Cities.FindAsync(id);
            city.IsDeleted = true;
            city.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CityExists(int id)
        {
            return this._context.Cities.Any(e => e.Id == id);
        }
    }
}
