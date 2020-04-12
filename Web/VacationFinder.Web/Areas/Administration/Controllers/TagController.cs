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
    using VacationFinder.Web.ViewModels.Administration.Tag;

    public class TagController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index(int? page, int? perPage, string? order, string? title, string? sort)
        {
            int pageSize = perPage ?? 5;
            int pageNumber = page ?? 1;

            var list = await this._context.Tags.ToListAsync();

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
            if (sort != null)
            {
                list = list.Where(t => t.Sort == int.Parse(sort)).ToList();
            }

            return this.View(new IndexViewModel() { List = GetPage(list, pageNumber, pageSize), Pages = GetPageCount(list, pageSize) });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var tag = await this._context.Tags
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
                this._context.Add(tag);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
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

            var tag = await this._context.Tags.FindAsync(id);
            if (tag == null)
            {
                return this.NotFound();
            }
            return this.View(tag);
        }

        // POST: Admin/Categories/Edit/5
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
                    this._context.Update(tag);
                    await this._context.SaveChangesAsync();
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

            var tag = await this._context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return this.NotFound();
            }

            return this.View(tag);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await this._context.Tags.FindAsync(id);
            tag.IsDeleted = true;
            tag.DeletedOn = DateTime.Now.AddHours(-3);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool TagExists(int id)
        {
            return this._context.Tags.Any(e => e.Id == id);
        }

        private static IEnumerable<Tag> GetPage(IEnumerable<Tag> list, int pageNumber, int pageSize = 10)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        private static int GetPageCount(IEnumerable<Tag> list, int pageSize = 10)
        {
            int count = list.Count();

            if (count % pageSize == 0)
            {
                return count / pageSize;
            }

            return (count / pageSize) + 1;
        }


    }
}
