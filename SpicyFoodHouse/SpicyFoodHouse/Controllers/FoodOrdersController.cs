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

    public class FoodOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public FoodOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public void GetDropdownData()
        {
            IQueryable<FoodType> foodType = from item in _context.FoodType
                orderby item.TypeName ascending
                select item;

            IQueryable<Food> foods = from item in _context.Food
                orderby item.FoodName ascending
                select item;

            IQueryable<FoodQuarter> foodQuarters = from item in _context.FoodQuarter
                orderby item.QuarterName ascending
                select item;

            IQueryable<PaymentMethod> paymentMethods = from item in _context.PaymentMethod
                orderby item.PaymentMethodName ascending
                select item;

            ViewBag.foodItemType = foodType;
            ViewBag.foods = foods;
            ViewBag.foodQuarters = foodQuarters;
            ViewBag.paymentMethods = paymentMethods;

        }


        //  Customer Information Show in the modal

        [HttpPost]
        public async Task<IActionResult> CustomerInfoSHow(int orderId)
        {

            var customerEmail =  _context.FoodOrder.FirstOrDefault(f => f.OrderId == orderId);

            string email = customerEmail.CustomerEmail;

            ApplicationUser applicationUser = await _userManager.FindByNameAsync(email);

            IQueryable<FoodOrder> foodOrders = from item in _context.FoodOrder
                where item.CustomerEmail == email
                select item;


            ViewBag.email = applicationUser.Email;
            ViewBag.name = applicationUser.CustomerName;
            ViewBag.phone = applicationUser.Phone;
            ViewBag.address = applicationUser.Address;
            ViewBag.totalOrdered = foodOrders.Count();
            ViewBag.IsVerified = foodOrders.Count();
            ViewBag.nid = applicationUser.NidOrBith;


            return PartialView("_CustomerInfoPartial");
        }

        // GET: FoodOrders
        public async Task<IActionResult> Index(string srctext, int sortId, int page = 1)
        {
            var applicationDbContext = _context.FoodOrder.Include(f => f.FoodQuarter).Include(f => f.FoodType).Include(f => f.PaymentMethod).OrderByDescending(g => g.OrderId);


            if (!string.IsNullOrEmpty(srctext))
            {
                applicationDbContext = (IOrderedQueryable<FoodOrder>) _context.FoodOrder.Include(f => f.FoodQuarter).Include(f => f.FoodType).Include(f => f.PaymentMethod).Where(g => g.CustomerEmail.Contains(srctext));
            }

            if (sortId == 1)
            {
                applicationDbContext = (IOrderedQueryable<FoodOrder>) from item in applicationDbContext where item.IsSeen == true select item;
            }
            else if (sortId == 2)
            {
                applicationDbContext = (IOrderedQueryable<FoodOrder>)from item in applicationDbContext where item.IsSeen == false select item;
            }

            var foodTypes = _context.FoodType;

            ViewBag.foodTypes = foodTypes;
            ViewBag.totalOrderCount = applicationDbContext.Count();

            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 10;

            ViewBag.totalCount = applicationDbContext.Count();


            return View(await PaginatedList<FoodOrder>.CreateAsync(applicationDbContext, page, pageSize));
        }

        // GET: FoodOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodOrder = await _context.FoodOrder
                .Include(f => f.FoodQuarter)
                .Include(f => f.FoodType)
                .Include(f => f.PaymentMethod)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (foodOrder == null)
            {
                return NotFound();
            }


            FoodOrder order = _context.FoodOrder.SingleOrDefault(f => f.OrderId == id);

            order.IsSeen = true;

            _context.FoodOrder.Update(order);
            _context.SaveChanges();


            return View(foodOrder);
        }

        // GET: FoodOrders/Create
        public IActionResult Create()
        {

            GetDropdownData();

            return View();
        }

        // POST: FoodOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerEmail,TypeId,FoodName,Price,NumberOfFood,QuarterId,TotalPrice,TypeId2,FoodName2,Price2,NumberOfFood2,QuarterId2,TotalPrice2,TypeId3,FoodName3,Price3,NumberOfFood3,QuarterId3,TotalPrice3,SubTotalPrice,PaymentMethodId,LastFiveDigit,IsPaid,IsSeen,IsAccepted,IsRejected,OrderDate,LastUpdatedDate")] FoodOrder foodOrder)
        {
            if (_signInManager.IsSignedIn(User))
            {
                foodOrder.CustomerEmail = _userManager.GetUserName(User);
            }
            else
            {
               return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrEmpty(foodOrder.CustomerEmail))
            {
                if (foodOrder.TypeId > 0 && foodOrder.PaymentMethodId > 0 && foodOrder.TotalPrice > 0)
                {
                    foodOrder.OrderDate = DateTime.Now;

                    _context.Add(foodOrder);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            GetDropdownData();

            return View(foodOrder);
        }

        // GET: FoodOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodOrder = await _context.FoodOrder.SingleOrDefaultAsync(m => m.OrderId == id);
            if (foodOrder == null)
            {
                return NotFound();
            }

            GetDropdownData();

            return View(foodOrder);
        }

        // POST: FoodOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerEmail,TypeId,FoodName,Price,NumberOfFood,QuarterId,TotalPrice,TypeId2,FoodName2,Price2,NumberOfFood2,QuarterId2,TotalPrice2,TypeId3,FoodName3,Price3,NumberOfFood3,QuarterId3,TotalPrice3,SubTotalPrice,PaymentMethodId,LastFiveDigit,IsPaid,IsSeen,IsAccepted,IsRejected,OrderDate,LastUpdatedDate")] FoodOrder foodOrder)
        {
            if (id != foodOrder.OrderId)
            {
                return NotFound();
            }

            if (foodOrder.TypeId > 0 && foodOrder.PaymentMethodId > 0 && foodOrder.TotalPrice > 0)
            {
                try
                {
                    _context.Update(foodOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodOrderExists(foodOrder.OrderId))
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
            //get dropdown data

            GetDropdownData();

            return View(foodOrder);
        }

        // GET: FoodOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodOrder = await _context.FoodOrder
                .Include(f => f.FoodQuarter)
                .Include(f => f.FoodType)
                .Include(f => f.PaymentMethod)
                .SingleOrDefaultAsync(m => m.OrderId == id);
            if (foodOrder == null)
            {
                return NotFound();
            }

            return View(foodOrder);
        }

        // POST: FoodOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodOrder = await _context.FoodOrder.SingleOrDefaultAsync(m => m.OrderId == id);
            _context.FoodOrder.Remove(foodOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodOrderExists(int id)
        {
            return _context.FoodOrder.Any(e => e.OrderId == id);
        }
    }
}
