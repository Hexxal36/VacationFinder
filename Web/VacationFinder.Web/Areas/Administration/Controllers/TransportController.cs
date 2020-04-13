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

    public class TransportController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        private readonly IPagingService pagingService;

        public TransportController(ApplicationDbContext context)
        {
            this._context = context;
            this.pagingService = new PagingService();
        }

        public async Task<IActionResult> Index(int? page, int? perPage, string? order, string? title, string? sort)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this._context.Transports.ToListAsync();

            if (order != null)
            {
                switch (order)
                {
                    case "title":
                        list = list.OrderBy(t => t.Title).ToList();
                        break;
                    case "sort":
                        list = list.OrderByDescending(t => t.Sort).ToList();
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

            if (title != null)
            {
                list = list.Where(t => t.Title.Contains(title)).ToList();
            }

            try
            {
                if (sort != null)
                {
                    list = list.Where(t => t.Sort == int.Parse(sort)).ToList();
                }
            }
            catch { }

            this.ViewBag.Pages = this.pagingService.GetPageCount(list, pageSize);

            return this.View(this.pagingService.GetPage(list, pageNumber, pageSize).Cast<Transport>().ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var transport = await this._context.Transports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return this.NotFound();
            }

            return this.View(transport);
        }

        // GET: Admin/Transport/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Admin/Transport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Sort,Id,IsActive")] Transport transport)
        {
            transport.CreatedOn = DateTime.Now.AddHours(-3);
            transport.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this._context.Add(transport);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }
            return this.View(transport);
        }

        // GET: Admin/Transport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var transport = await this._context.Transports.FindAsync(id);
            if (transport == null)
            {
                return this.NotFound();
            }
            return this.View(transport);
        }

        // POST: Admin/Transport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Sort,Id,IsActive")] Transport transport)
        {


            if (id != transport.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(transport);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.TransportExists(transport.Id))
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

            return this.View(transport);
        }

        // GET: Admin/Transport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var transport = await this._context.Transports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return this.NotFound();
            }

            return this.View(transport);
        }

        // POST: Admin/Transport/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transport = await this._context.Transports.FindAsync(id);
            transport.IsDeleted = true;
            transport.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool TransportExists(int id)
        {
            return this._context.Transports.Any(e => e.Id == id);
        }
    }
}