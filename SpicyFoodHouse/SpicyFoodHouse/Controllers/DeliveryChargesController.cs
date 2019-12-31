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

    public class DeliveryChargesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public DeliveryChargesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: DeliveryCharges
            public async Task<IActionResult> Index()
        {
            return View(await _context.DeliveryCharge.ToListAsync());
        }

        // GET: DeliveryCharges/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryCharge = await _context.DeliveryCharge
                .SingleOrDefaultAsync(m => m.Id == id);
            if (deliveryCharge == null)
            {
                return NotFound();
            }

            return View(deliveryCharge);
        }

        // GET: DeliveryCharges/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DeliveryCharges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationRange,Charge,Description,ManagerSignature,EntryDate,LastUpdatedDate")] DeliveryCharge deliveryCharge)
        {

            deliveryCharge.EntryDate = DateTime.Now;

            deliveryCharge.ManagerSignature = _userManager.GetUserName(User);

            if (ModelState.IsValid)
            {
                _context.Add(deliveryCharge);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryCharge);
        }

        // GET: DeliveryCharges/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryCharge = await _context.DeliveryCharge.SingleOrDefaultAsync(m => m.Id == id);
            if (deliveryCharge == null)
            {
                return NotFound();
            }
            return View(deliveryCharge);
        }

        // POST: DeliveryCharges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LocationRange,Charge,Description,ManagerSignature,EntryDate,LastUpdatedDate")] DeliveryCharge deliveryCharge)
        {
            if (id != deliveryCharge.Id)
            {
                return NotFound();
            }

            deliveryCharge.LastUpdatedDate = DateTime.Now;
            deliveryCharge.ManagerSignature = _userManager.GetUserName(User);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryCharge);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryChargeExists(deliveryCharge.Id))
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
            return View(deliveryCharge);
        }

        // GET: DeliveryCharges/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryCharge = await _context.DeliveryCharge
                .SingleOrDefaultAsync(m => m.Id == id);
            if (deliveryCharge == null)
            {
                return NotFound();
            }

            return View(deliveryCharge);
        }

        // POST: DeliveryCharges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryCharge = await _context.DeliveryCharge.SingleOrDefaultAsync(m => m.Id == id);
            _context.DeliveryCharge.Remove(deliveryCharge);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryChargeExists(int id)
        {
            return _context.DeliveryCharge.Any(e => e.Id == id);
        }
    }
}
