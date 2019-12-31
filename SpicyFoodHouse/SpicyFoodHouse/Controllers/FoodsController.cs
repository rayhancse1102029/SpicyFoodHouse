using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]

    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public FoodsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Foods
        public async Task<IActionResult> Index(int page = 1)
        {
            var applicationDbContext = _context.Food.Include(f => f.FoodType);

            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 10;

            return View(await PaginatedList<Food>.CreateAsync(applicationDbContext,page,pageSize));
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.FoodType)
                .SingleOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.FoodType, "TypeId", "TypeName");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FoodId,FoodName,TypeId,Image,Description,Price,AvailableInStock,Discount,IsTranding,IsPopular,IsDiscounted,ManagerSignature,EntryDate,LastUpdatedDate")] Food food, IFormFile foodImage)
        {
            if (food.TypeId < 0)
            {
                return View(food);
            }


            Food foodExists = _context.Food.FirstOrDefault(f => f.FoodName == food.FoodName);


            if (!string.IsNullOrEmpty(foodExists.FoodName))
            {
                ViewBag.existsErrorMessage = "This food already exists.";
                return View(food);
            }

            if (!string.IsNullOrEmpty(food.FoodName) && !string.IsNullOrEmpty(Convert.ToString(food.TypeId)) && !string.IsNullOrEmpty(food.Description) && !string.IsNullOrEmpty(Convert.ToString(food.Price)) && !string.IsNullOrEmpty(Convert.ToString(food.AvailableInStock)) && !string.IsNullOrEmpty(Convert.ToString(food.Discount)))
            {

                if (foodImage.Length > 0)
                {
                    byte[] p1 = null;

                    using (var fs1 = foodImage.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();

                        food.Image = p1;
                    }
                }

                string ue = HttpContext.Session.GetString("userEmail");

                if (!string.IsNullOrEmpty(ue))
                {
                    food.ManagerSignature = ue;
                }
                else
                {
                  return  RedirectToAction("Login", "Account");
                }


                food.EntryDate = DateTime.Now;

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.FoodType, "TypeId", "TypeName", food.TypeId);
            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.SingleOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.FoodType, "TypeId", "TypeName", food.TypeId);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FoodId,FoodName,TypeId,Image,Description,Price,AvailableInStock,Discount,IsTranding,IsPopular,IsDiscounted,ManagerSignature,EntryDate,LastUpdatedDate")] Food food, IFormFile foodImage)
        {

            if (id != food.FoodId)
            {
                return NotFound();
            }

            if (food.TypeId < 0)
            {
                return View(food);
            }

            string foodTypeId = Convert.ToString(food.TypeId);
            string price = Convert.ToString(food.Price);
            string availableInStock = Convert.ToString(food.AvailableInStock);
            string discount = Convert.ToString(food.Discount);


            if (!string.IsNullOrEmpty(food.FoodName) && !string.IsNullOrEmpty(foodTypeId) && !string.IsNullOrEmpty(food.Description) && !string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(availableInStock) && !string.IsNullOrEmpty(discount))
            {

                if (foodImage.Length > 0)
                {
                    byte[] p1 = null;

                    using (var fs1 = foodImage.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();

                        food.Image = p1;
                    }
                }

                string ue = HttpContext.Session.GetString("userEmail");

                if (!string.IsNullOrEmpty(ue))
                {
                    food.ManagerSignature = ue;
                }
                else
                {
                  return  RedirectToAction("Login", "Account");
                }
                try
                {
                    food.LastUpdatedDate = DateTime.Now;
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.FoodId))
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
            ViewData["TypeId"] = new SelectList(_context.FoodType, "TypeId", "TypeName", food.TypeId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.FoodType)
                .SingleOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.SingleOrDefaultAsync(m => m.FoodId == id);
            _context.Food.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.FoodId == id);
        }
    }
}
