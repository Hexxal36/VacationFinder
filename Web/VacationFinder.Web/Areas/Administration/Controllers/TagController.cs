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

    public class TagController : AdministrationController
    {
        private readonly ApplicationDbContext context;

        private readonly IPagingService pagingService;

        public TagController(ApplicationDbContext context)
        {
            this.context = context;
            this.pagingService = new PagingService();
        }

        #nullable enable

        public async Task<IActionResult> Index(int? page, int? perPage, string? order, string? title, string? sort)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this.context.Tags.ToListAsync();

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
            catch
            {
            }

            this.ViewBag.Pages = this.pagingService.GetPageCount(list, pageSize);

            return this.View(this.pagingService.GetPage(list, pageNumber, pageSize).Cast<Tag>().ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.View(tag);
        }

        // GET: Admin/Tag/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Admin/Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Sort,Id,IsActive")] Tag tag)
        {
            tag.CreatedOn = DateTime.Now.AddHours(-3);
            tag.IsDeleted = false;

            if (this.ModelState.IsValid)
            {
                this.context.Add(tag);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Create));
            }

            return this.View(tag);
        }

        // GET: Admin/Tag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags.FindAsync(id);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.View(tag);
        }

        // POST: Admin/Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Sort,Id,IsActive")] Tag tag)
        {
            if (id != tag.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.context.Update(tag);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.TagExists(tag.Id))
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

            return this.View(tag);
        }

        // GET: Admin/Tag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.View(tag);
        }

        // POST: Admin/Tag/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await this.context.Tags.FindAsync(id);
            tag.IsDeleted = true;
            tag.DeletedOn = DateTime.Now.AddHours(-3);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool TagExists(int id)
        {
            return this.context.Tags.Any(e => e.Id == id);
        }
    }
}
