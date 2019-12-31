using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]

    public class AvailableInStocksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public AvailableInStocksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: AvailableInStocks
            public async Task<IActionResult> Index()
        {
            return View(await _context.AvailableInStock.ToListAsync());
        }

        // GET: AvailableInStocks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availableInStock = await _context.AvailableInStock
                .SingleOrDefaultAsync(m => m.Id == id);
            if (availableInStock == null)
            {
                return NotFound();
            }

            return View(availableInStock);
        }

        // GET: AvailableInStocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AvailableInStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoodName,AvailableFood,Description,ManagerSignature,EntryDate,LastUpdatedDate")] AvailableInStock availableInStock)
        {

            availableInStock.EntryDate = DateTime.Now;
            availableInStock.ManagerSignature = _userManager.GetUserName(User);


            if (ModelState.IsValid)
            {
                _context.Add(availableInStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(availableInStock);
        }

        // GET: AvailableInStocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availableInStock = await _context.AvailableInStock.SingleOrDefaultAsync(m => m.Id == id);
            if (availableInStock == null)
            {
                return NotFound();
            }
            return View(availableInStock);
        }

        // POST: AvailableInStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoodName,AvailableFood,Description,ManagerSignature,EntryDate,LastUpdatedDate")] AvailableInStock availableInStock)
        {
            if (id != availableInStock.Id)
            {
                return NotFound();
            }

            availableInStock.LastUpdatedDate = DateTime.Now;
            availableInStock.ManagerSignature = _userManager.GetUserName(User);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(availableInStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvailableInStockExists(availableInStock.Id))
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
            return View(availableInStock);
        }

        // GET: AvailableInStocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availableInStock = await _context.AvailableInStock
                .SingleOrDefaultAsync(m => m.Id == id);
            if (availableInStock == null)
            {
                return NotFound();
            }

            return View(availableInStock);
        }

        // POST: AvailableInStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var availableInStock = await _context.AvailableInStock.SingleOrDefaultAsync(m => m.Id == id);
            _context.AvailableInStock.Remove(availableInStock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvailableInStockExists(int id)
        {
            return _context.AvailableInStock.Any(e => e.Id == id);
        }
    }
}
