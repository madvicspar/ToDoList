using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: ItemCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }

        // GET: ItemCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        // GET: ItemCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.Categories.FindAsync(id);
            if (itemCategory == null)
            {
                return NotFound();
            }
            return View(itemCategory);
        }

        // POST: ItemCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title")] ItemCategory itemCategory)
        {
            if (id != itemCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCategoryExists(itemCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        // GET: ItemCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }

        // POST: ItemCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var itemCategory = await _context.Categories.FindAsync(id);
            if (itemCategory != null)
            {
                _context.Categories.Remove(itemCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemCategoryExists(long id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
