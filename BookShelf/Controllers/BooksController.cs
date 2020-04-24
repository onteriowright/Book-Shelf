using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelf.Data;
using BookShelf.Models;
using BookShelf.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShelf.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var user = await GetUserAsync();
            var books = await _context.Book
                .Where(b => b.ApplicationUserId == user.Id)
                .Include(b => b.ListOfBookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToListAsync();

            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new BookFormViewModel();
            var genreOptions = await _context.Genre
                  .Select(g => new SelectListItem()
                  {
                      Text = g.Name,
                      Value = g.Id.ToString()
                  })
                  .ToListAsync();

            viewModel.GenreOptions = genreOptions;

            return View(viewModel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookFormViewModel book)
        {
            try
            {
                var user = await GetUserAsync();
                var newBook = new Book()
                {
                    Title = book.Title,
                    Authur = book.Authur,
                    ApplicationUserId = user.Id
                };

                newBook.ListOfBookGenres = book.SelectedGenreIds
                    .Select(genreId => new BookGenre()
                    {
                        Book = newBook,
                        GenreId = genreId
                    }).ToList();

                _context.Book.Add(newBook);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Books/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<ApplicationUser> GetUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}