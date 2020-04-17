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
    public class HotelController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.Hotels.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hotel = await this._context.Hotels
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
            this.ViewBag.Cities = await this._context.Cities.ToListAsync();

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
                this._context.Add(hotel);
                await this._context.SaveChangesAsync();
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

            var hotel = await this._context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Cities = await this._context.Cities.ToListAsync();

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
                    this._context.Update(hotel);
                    await this._context.SaveChangesAsync();
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

            Hotel hotel = await this._context.Hotels
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
            var hotel = await this._context.Hotels.FindAsync(id);
            hotel.IsDeleted = true;
            hotel.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool HotelExists(int id)
        {
            return this._context.Hotels.Any(e => e.Id == id);
        }
    }
}
