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
    public class OfferImageController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public OfferImageController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Create()
        {
            this.ViewBag.Offers = await this._context.Offers.ToListAsync();

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageUrl,Id,OfferId")] OfferImage offerImg)
        {
            offerImg.CreatedOn = DateTime.Now.AddHours(-3);
            offerImg.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this._context.Add(offerImg);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }

            this.ViewBag.Offers = await this._context.Offers.ToListAsync();

            return this.View(offerImg);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var offerImg = await this._context.OfferImages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerImg == null)
            {
                return this.NotFound();
            }

            return this.View(offerImg);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offerImg = await this._context.OfferImages.FindAsync(id);
            offerImg.IsDeleted = true;
            offerImg.DeletedOn = DateTime.Now.AddHours(-3);
            var offerId = offerImg.OfferId;

            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Details", "Offer", new { area = "Administration", id = offerId });
        }
    }
}
