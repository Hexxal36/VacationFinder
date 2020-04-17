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

    public class HotelReviewController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public HotelReviewController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return this.View(await this._context.HotelReviews.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var review = await this._context.HotelReviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return this.NotFound();
            }

            return this.View(review);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            HotelReview review = await this._context.HotelReviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return this.NotFound();
            }

            return this.View(review);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await this._context.HotelReviews.FindAsync(id);
            review.IsDeleted = true;
            review.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var review = await this._context.HotelReviews.FindAsync(id);
            if (review == null)
            {
                return this.NotFound();
            }

            return this.View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title, Body, Id, Grade, UserId, HotelId")] HotelReview review)
        {
            if (id != review.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(review);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.HotelReviewExists(review.Id))
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

            return this.View(review);
        }

        private bool HotelReviewExists(int id)
        {
            return this._context.HotelReviews.Any(e => e.Id == id);
        }
    }
}
