using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class FoodQuartersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodQuartersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FoodQuarters
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodQuarter.ToListAsync());
        }

        // GET: FoodQuarters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodQuarter = await _context.FoodQuarter
                .SingleOrDefaultAsync(m => m.QuarterId == id);
            if (foodQuarter == null)
            {
                return NotFound();
            }

            return View(foodQuarter);
        }

        // GET: FoodQuarters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodQuarters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuarterId,QuarterName,ManagerSignature,EntryDate,LastUpdatedDate")] FoodQuarter foodQuarter)
        {
            if (ModelState.IsValid)
            {
                foodQuarter.EntryDate = DateTime.Now;
                _context.Add(foodQuarter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodQuarter);
        }

        // GET: FoodQuarters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodQuarter = await _context.FoodQuarter.SingleOrDefaultAsync(m => m.QuarterId == id);
            if (foodQuarter == null)
            {
                return NotFound();
            }
            return View(foodQuarter);
        }

        // POST: FoodQuarters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuarterId,QuarterName,ManagerSignature,EntryDate,LastUpdatedDate")] FoodQuarter foodQuarter)
        {
            if (id != foodQuarter.QuarterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foodQuarter.LastUpdatedDate = DateTime.Now;
                    _context.Update(foodQuarter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodQuarterExists(foodQuarter.QuarterId))
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
            return View(foodQuarter);
        }

        // GET: FoodQuarters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodQuarter = await _context.FoodQuarter
                .SingleOrDefaultAsync(m => m.QuarterId == id);
            if (foodQuarter == null)
            {
                return NotFound();
            }

            return View(foodQuarter);
        }

        // POST: FoodQuarters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodQuarter = await _context.FoodQuarter.SingleOrDefaultAsync(m => m.QuarterId == id);
            _context.FoodQuarter.Remove(foodQuarter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodQuarterExists(int id)
        {
            return _context.FoodQuarter.Any(e => e.QuarterId == id);
        }
    }
}
