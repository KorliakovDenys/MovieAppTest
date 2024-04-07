using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using MovieCatalog.Data.Models;
using MovieCatalog.DTO;
using MovieCatalog.Repository;

namespace MovieCatalog.Controllers {
    public class CategoryController(IRepository<Category> repository) : Controller {
        public async Task<IActionResult> Index() {
            var categories = repository.GetAll()?.Include(c => c.ParentCategory);
            if (categories != null && categories.Any()) {
                return View(await categories.ToListAsync());
            }

            return View();
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var category = await repository.FindAsync((int)id);

            if (category == null) {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create() {
            SetFilmParentCategories();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentCategoryId")] Category category) {
            if (ModelState.IsValid) {
                await repository.InsertAsync(category);
                return RedirectToAction(nameof(Index));
            }

            SetFilmParentCategories();
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null || !CategoryExists((int)id)) {
                return NotFound();
            }

            var category = await repository.FindAsync((int)id);
            if (category == null) {
                return NotFound();
            }

            SetFilmParentCategories();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentCategoryId")] Category category) {
            if (id != category.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    await repository.UpdateAsync(category);
                }
                catch (DbUpdateConcurrencyException) {
                    if (!CategoryExists(category.Id)) {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            SetFilmParentCategories();
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var category = await repository.FindAsync((int)id);
            if (category == null) {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await repository.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id) {
            return repository.AnyAsync(id).Result;
        }

        private void SetFilmParentCategories() {
            var categories = new List<TextValueDTO> { new("", -1) };
            if (repository.AnyAsync().Result) {
                categories.AddRange(repository.GetAll().Select(c => new TextValueDTO(c.Name, c.Id)).ToList());
            }

            ViewData["ParentCategoryId"] = new SelectList(categories, "Value", "Text");
        }
    }
}