using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;


namespace SpicyFoodHouse.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminPanelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index(int id)
        {

            var food =  _context.Food;
            var foodType = _context.FoodType;

            double amount = 0;



            var comment = from item in _context.Comment where item.CommentTime.Date == DateTime.Today select item;
            var order = from item in _context.FoodOrder where item.OrderDate.Date == DateTime.Today select item;

            foreach (var item in _context.FoodOrder)
            {
                if (item.OrderDate.Date == DateTime.Today)
                {
                    amount += item.SubTotalPrice;
                }
            }


            if (id == 7)
            {
                 comment = from item in _context.Comment where item.CommentTime.Date > DateTime.Now.AddDays(-7) select item;
                 order = from item in _context.FoodOrder where item.OrderDate.Date > DateTime.Now.AddDays(-7) select item;

                foreach (var item in _context.FoodOrder)
                {
                    if (item.OrderDate.Date > DateTime.Now.AddDays(-7))
                    {
                        amount += item.SubTotalPrice;
                    }
                }

            }
            else if (id == 30)
            {
                comment = from item in _context.Comment where item.CommentTime.Date > DateTime.Now.AddDays(-30) select item;
                order = from item in _context.FoodOrder where item.OrderDate.Date > DateTime.Now.AddDays(-30) select item;

                foreach (var item in _context.FoodOrder)
                {
                    if (item.OrderDate.Date > DateTime.Now.AddDays(-30))
                    {
                        amount += item.SubTotalPrice;
                    }
                }

            }
            else if (id == 365)
            {
                comment = from item in _context.Comment where item.CommentTime.Date > DateTime.Now.AddDays(-365) select item;
                order = from item in _context.FoodOrder where item.OrderDate.Date > DateTime.Now.AddDays(-365) select item;

                foreach (var item in _context.FoodOrder)
                {
                    if (item.OrderDate.Date > DateTime.Now.AddDays(-7))
                    {
                        amount += item.SubTotalPrice;
                    }
                }

            }


            ViewBag.food = food.Count();
            ViewBag.foodType = foodType.Count();
            ViewBag.order = order.Count();
            ViewBag.comment = comment.Count();
            ViewBag.amount = amount;


            return View();
        }

        public async Task<IActionResult> GetAllUser(string srctext, int page = 1)
        {
            IQueryable<ApplicationUser> db = from item in _context.Users
                orderby item.Id descending
                select item;


            if (!string.IsNullOrEmpty(srctext))
            {
                srctext = srctext.ToUpper();

                db = from item in db
                    orderby item.Id descending
                    where (item.CustomerName.ToUpper().Contains(srctext) || item.Email.ToUpper().Contains(srctext))
                    select item;
            }

            ViewBag.TotalCount = db.Count();
            ViewBag.srctext = srctext;

            if (page <= 0) { page = 1; }
            int pageSize = 10;

            return View(await PaginatedList<ApplicationUser>.CreateAsync(db, page, pageSize));
        }
    }
}