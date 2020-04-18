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
    public class OfferController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public OfferController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.Offers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var offer = await this._context.Offers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return this.NotFound();
            }

            return this.View(offer);
        }

        // GET: Admin/Offer/Create
        public async Task<IActionResult> Create()
        {
            this.ViewBag.Hotels = await this._context.Hotels.Where(x => x.IsActive).ToListAsync();
            this.ViewBag.Tags = await this._context.Tags.Where(x => x.IsActive).ToListAsync();
            this.ViewBag.Transports = await this._context.Transports.Where(x => x.IsActive).ToListAsync();

            return this.View();
        }

        // POST: Admin/Offer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title, Price, Id, IsActive, Days, Nights, Places, IsSpecial, Active_From, Active_Until, Description, HotelId, TagId, TransportId")] Offer offer)
        {
            offer.CreatedOn = DateTime.Now.AddHours(-3);
            offer.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this._context.Add(offer);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(offer);
        }

        // GET: Admin/Offer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var offer = await this._context.Offers.FindAsync(id);
            if (offer == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hotels = await this._context.Hotels.Where(x => x.IsActive).ToListAsync();
            this.ViewBag.Tags = await this._context.Tags.Where(x => x.IsActive).ToListAsync();
            this.ViewBag.Transports = await this._context.Transports.Where(x => x.IsActive).ToListAsync();

            return this.View(offer);
        }

        // POST: Admin/Offer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Title, Price, Id, IsActive, Days, Nights, Places, IsSpecial, Active_From, Active_Until, Description, HotelId, TagId, TransportId")] Offer offer)
        {
            if (id != offer.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(offer);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.OfferExists(offer.Id))
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

            return this.View(offer);
        }

        // GET: Admin/Offer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var offer = await this._context.Offers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return this.NotFound();
            }

            return this.View(offer);
        }

        // POST: Admin/Offer/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await this._context.Offers.FindAsync(id);
            offer.IsDeleted = true;
            offer.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool OfferExists(int id)
        {
            return this._context.Offers.Any(e => e.Id == id);
        }
    }
}
