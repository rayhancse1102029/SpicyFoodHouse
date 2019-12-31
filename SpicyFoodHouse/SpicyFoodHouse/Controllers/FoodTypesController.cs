using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class FoodTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FoodTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodType.ToListAsync());
        }

        // GET: FoodTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType
                .SingleOrDefaultAsync(m => m.TypeId == id);
            if (foodType == null)
            {
                return NotFound();
            }

            return View(foodType);
        }

        // GET: FoodTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName,ManagerSignature,EntryDate,LastUpdatedDate")] FoodType foodType)
        {
            if (!string.IsNullOrEmpty(foodType.TypeName))
            {

                string ue = HttpContext.Session.GetString("userEmail");

                if (!string.IsNullOrEmpty(ue))
                {
                    foodType.ManagerSignature = ue;
                }
                else
                {
                   return RedirectToAction("Login", "Account");
                }

                foodType.EntryDate = DateTime.Now;
                _context.Add(foodType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodType);
        }

        // GET: FoodTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType.SingleOrDefaultAsync(m => m.TypeId == id);
            if (foodType == null)
            {
                return NotFound();
            }
            return View(foodType);
        }

        // POST: FoodTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName,ManagerSignature,EntryDate,LastUpdatedDate")] FoodType foodType)
        {
            if (id != foodType.TypeId)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(foodType.TypeName))
            {
                string ue = HttpContext.Session.GetString("userEmail");

                if (!string.IsNullOrEmpty(ue))
                {
                    foodType.ManagerSignature = ue;
                    foodType.LastUpdatedDate = DateTime.Now;
                }
                else
                {
                   return RedirectToAction("Login", "Account");
                }


                try
                {
                    foodType.LastUpdatedDate = DateTime.Now;
                    _context.Update(foodType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodTypeExists(foodType.TypeId))
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
            return View(foodType);
        }

        // GET: FoodTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType
                .SingleOrDefaultAsync(m => m.TypeId == id);
            if (foodType == null)
            {
                return NotFound();
            }

            return View(foodType);
        }

        // POST: FoodTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodType = await _context.FoodType.SingleOrDefaultAsync(m => m.TypeId == id);
            _context.FoodType.Remove(foodType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodTypeExists(int id)
        {
            return _context.FoodType.Any(e => e.TypeId == id);
        }
    }
}
