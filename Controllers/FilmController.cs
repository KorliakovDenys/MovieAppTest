using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data.Models;
using MovieCatalog.Repository;

namespace MovieCatalog.Controllers {
    public class FilmController(IRepository<Film> repository) : Controller {
        public async Task<IActionResult> Index() {
            if (await repository.AnyAsync()) {
                return View(await repository.GetAll().ToListAsync());
            }

            return View();
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var film = await repository
                .FindAsync((int)id);
            if (film == null) {
                return NotFound();
            }

            return View(film);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Director,ReleaseDate")] Film film) {
            if (!ModelState.IsValid) return View(film);

            await repository.InsertAsync(film);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null)
                return NotFound();

            var film = await repository.FindAsync((int)id);
            if (film == null)
                return NotFound();

            return View(film);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Director,ReleaseDate")] Film film) {
            if (id != film.Id)
                return NotFound();

            if (!ModelState.IsValid) return View(film);

            try {
                await repository.UpdateAsync(film);
            }
            catch (DbUpdateConcurrencyException) {
                if (!FilmExists(film.Id)) {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null)
                return NotFound();

            var film = await repository.FindAsync((int)id);
            if (film == null)
                return NotFound();

            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await repository.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id) {
            return repository.AnyAsync(id).Result;
        }
    }
}