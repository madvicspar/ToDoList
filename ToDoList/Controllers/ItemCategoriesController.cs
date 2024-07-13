using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemCategoriesController : Controller
    {
        /// <summary>
        /// Displays a page with a list of all item categories
        /// </summary>
        /// <returns></returns>
        // GET: ItemCategories
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                return dataBase == null ? View() : View(await dataBase.Categories
                    .Where(x => x.UserId == currentUserId)
                    .ToListAsync());
            }
        }

        /// <summary>
        /// Displays the details of a specific item category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ItemCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase == null)
                {
                    return NotFound();
                }

                var itemCategory = await dataBase.Categories.FirstOrDefaultAsync(m => m.Id == id);
                return itemCategory == null ? NotFound() : View(itemCategory);
            }
        }

        /// <summary>
        /// Displays a confirmation page for creating an item category
        /// </summary>
        /// <returns></returns>
        // GET: ItemCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new item category in the database
        /// </summary>
        /// <param name="itemCategory"></param>
        /// <returns></returns>
        // POST: ItemCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UserId")] ItemCategory itemCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(itemCategory);
            }

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase == null)
                {
                    return BadRequest("Database not found");
                }

                dataBase.Add(itemCategory);
                await dataBase.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays a confirmation page for editing an item category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ItemCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            // обработать случай, если категория с таким Id у другого пользователя
            if (id == null)
            {
                return NotFound();
            }

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase == null)
                {
                    return NotFound();
                }

                var itemCategory = await dataBase.Categories.FindAsync(id);
                return itemCategory == null ? NotFound() : View(itemCategory);
            }
        }

        /// <summary>
        /// Edits an existing item category in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemCategory">The updated item category object with modified properties</param>
        /// <returns></returns>
        // POST: ItemCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,UserId")] ItemCategory itemCategory)
        {
            if (id != itemCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var serviceScope = ServiceActivator.GetScope())
                {
                    var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    if (dataBase != null)
                    {
                        dataBase.Update(itemCategory);
                        await dataBase.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        /// <summary>
        /// Displays a confirmation page for deleting an item category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ItemCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase != null)
                {
                    var itemCategory = await dataBase.Categories.FirstOrDefaultAsync(m => m.Id == id);
                    return itemCategory == null ? NotFound() : View(itemCategory);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes an item category from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ItemCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase != null)
                {
                    var itemCategory = await dataBase.Categories.FindAsync(id);
                    if (itemCategory != null)
                    {
                        dataBase.Categories.Remove(itemCategory);
                        await dataBase.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Checks if an item category exists in the database based
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ItemCategoryExists(long id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                return dataBase?.Categories.Any(e => e.Id == id) ?? false;
            }
        }
    }
}
