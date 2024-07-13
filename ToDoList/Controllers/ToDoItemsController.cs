using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        /// <summary>
        /// Displays a page with a list of all to do items
        /// </summary>
        /// <returns></returns>
        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                return dataBase == null ? View() : View(await dataBase.ToDoItems
                    .Include(t => t.Category)
                    .Where(x => x.Category.UserId == currentUserId)
                    .ToListAsync());
            }
        }

        /// <summary>
        ///  Displays the details of a specific to do item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ToDoItems/Details/5
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

                var toDoItem = await dataBase.ToDoItems
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
                return toDoItem == null ? NotFound() : View(toDoItem);
            }
        }

        /// <summary>
        /// Displays a confirmation page for creating a to do item
        /// </summary>
        /// <returns></returns>
        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            var dataBase = ServiceActivator.GetScope().ServiceProvider.GetService<ApplicationDbContext>();
            ViewData["CategoryId"] = new SelectList(dataBase.Categories, "Id", "Title");
            return View();
        }

        /// <summary>
        /// Creates a new to do item in the database
        /// </summary>
        /// <param name="toDoItem"></param>
        /// <returns></returns>
        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsComplete,CategoryId")] ToDoItem toDoItem)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase == null)
                {
                    return View(toDoItem);
                }

                if (!ModelState.IsValid)
                {
                    ViewData["CategoryId"] = new SelectList(dataBase.Categories, "Id", "Title", toDoItem.CategoryId);
                    return View(toDoItem);
                }

                dataBase.Add(toDoItem);
                await dataBase.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays a confirmation page for editing a to do item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataBase = ServiceActivator.GetScope().ServiceProvider.GetService<ApplicationDbContext>();
            if (dataBase == null)
            {
                return NotFound();
            }

            var toDoItem = await dataBase.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(dataBase.Categories, "Id", "Title", toDoItem.CategoryId);
            return View(toDoItem);
        }

        /// <summary>
        /// Edits an existing to do item in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toDoItem"></param>
        /// <returns></returns>
        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,IsComplete,CategoryId")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase != null)
                {
                    if (!ModelState.IsValid)
                    {
                        ViewData["CategoryId"] = new SelectList(dataBase.Categories, "Id", "Title", toDoItem.CategoryId);
                        return View(toDoItem);
                    }

                    dataBase.Update(toDoItem);
                    await dataBase.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Displays a confirmation page for deleting a to do item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

                var toDoItem = await dataBase.ToDoItems
                    .FirstOrDefaultAsync(m => m.Id == id);

                return toDoItem == null ? NotFound() : View(toDoItem);
            }
        }

        /// <summary>
        /// Deletes an to do item from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (dataBase != null)
                {
                    var toDoItem = await dataBase.ToDoItems.FindAsync(id);
                    if (toDoItem != null)
                    {
                        dataBase.ToDoItems.Remove(toDoItem);
                        await dataBase.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Checks if an to do item exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ToDoItemExists(long id)
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                var dataBase = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                return dataBase?.ToDoItems.Any(e => e.Id == id) ?? false;
            }
        }
    }
}
