using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace SpicyFoodHouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {

            IQueryable<Food> trandingFoodList = from item in _context.Food
                orderby item.FoodName ascending
                where item.IsTranding == true
                select item;

            IQueryable<Food> popularFoodList = from item in _context.Food
                orderby item.FoodName ascending
                where item.IsPopular == true
                select item;

            IQueryable<Food> discountFoodList = from item in _context.Food
                orderby item.FoodName ascending
                where item.IsDiscounted == true
                select item;


            ViewBag.trandingFoodList = trandingFoodList;
            ViewBag.popularFoodList = popularFoodList;
            ViewBag.discountedFoodList = discountFoodList;


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> About(int page = 1)
        {

            IQueryable<Comment> comment = from item in _context.Comment
                orderby item.CommentTime select item;


            ViewBag.totalComment = comment.Count();
            if (page <= 0) { page = 1; }
            int pageSize = 10;

            return View(await PaginatedList<Comment>.CreateAsync(comment, page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AllFood(string id, int page = 1)
        {
            IQueryable<Food> applicationDbContext = _context.Food.Include(f => f.FoodType);


            if (!string.IsNullOrEmpty(id))
            {
                applicationDbContext = _context.Food.Where(f => f.FoodName.Contains(id));
            }


            IQueryable<FoodType> foodTypes = _context.FoodType;

            ViewBag.foodTypeList = foodTypes;

            ViewBag.totalCount = applicationDbContext.Count();

            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 10;

            return View(await PaginatedList<Food>.CreateAsync(applicationDbContext, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> AllFood(string srctext, int sortId, int foodTypeId, int page = 1)
        {
            IQueryable<Food> applicationDbContext = _context.Food.Include(f => f.FoodType);

            IQueryable<FoodType> foodTypes =  _context.FoodType;


            if (!string.IsNullOrEmpty(srctext))
            {
                srctext = srctext.ToLower();

                applicationDbContext = from item in _context.Food
                    where item.FoodName.ToLower().Contains(srctext) select item;
            }
            else if (sortId > 0)
            {
                if (sortId == 1)
                {
                    applicationDbContext = from item in _context.Food
                        where item.IsTranding == true
                        select item;
                }
                if (sortId == 2)
                {
                    applicationDbContext = from item in _context.Food
                        where item.IsPopular == true
                        select item;
                }
                if (sortId == 3)
                {
                    applicationDbContext = from item in _context.Food
                        where item.IsDiscounted == true
                        select item;
                }
            }
            else if (foodTypeId > 0)
            {
                applicationDbContext = from item in _context.Food
                    where item.TypeId == foodTypeId
                    select item;
            }

            ViewBag.foodTypeList = foodTypes;


            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 10;

            return View(await PaginatedList<Food>.CreateAsync(applicationDbContext, page, pageSize));
        }

        [HttpPost]
        public async Task<JsonResult> FoodIsAvailable(int foodId)
        {

            string result = "";

            Food food = await _context.Food.FirstOrDefaultAsync(f => f.FoodId == foodId);

            var isAvailable = food.AvailableInStock;

            if (Convert.ToInt32(isAvailable) > 0)
            {
                result = "success";
            }
            else if (Convert.ToInt32(isAvailable) <= 0)
            {
                result = "error";
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SelelctedOrderItemInfo( int id)
        {

            Food food = await _context.Food.FirstOrDefaultAsync(f => f.FoodId == id);

            FoodType foodType = await _context.FoodType.FirstOrDefaultAsync(f => f.TypeId == food.TypeId);

            ViewBag.foodName = food.FoodName;
            ViewBag.foodTypeName = foodType.TypeName;
            ViewBag.image = food.Image;
            ViewBag.discount = food.Discount;
            ViewBag.price = food.Price;
            ViewBag.description = food.Description;



            return PartialView("_SelectedOrderItemInfoPartial");
        }

        [HttpGet]
        public IActionResult OrderNow()
        {
            var applicationDbContext = _context.Food.Include(f => f.FoodType);

            ViewBag.foodList = applicationDbContext;
            ViewBag.totalCount = applicationDbContext.Count();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> OrderNow(int foodId1, int foodId2, int foodId3, int numberOfFood1, int numberOfFood2, int numberOfFood3, int quarterId1, int quarterId2, int quarterId3)
        {


            Food foodOne = await _context.Food.FirstOrDefaultAsync(f => f.FoodId == foodId1);
            Food foodTwo = await _context.Food.FirstOrDefaultAsync(f => f.FoodId == foodId2);
            Food foodThree = await _context.Food.FirstOrDefaultAsync(f => f.FoodId == foodId3);


            if (Convert.ToInt32(foodOne.AvailableInStock) <= 0 || Convert.ToInt32(foodTwo.AvailableInStock) <= 0 || Convert.ToInt32(foodThree.AvailableInStock) <= 0)
            {
                return Error();
            }

            if (_signInManager.IsSignedIn(User))
            {

                FoodOrder model = new FoodOrder();

                model.CustomerEmail =  _userManager.GetUserName(User);

                if (foodId1 > 0 || foodId2 > 0 || foodId3 > 0)
                {


                    if (foodId1 > 0 && numberOfFood1 > 0 && numberOfFood1 > 0)
                    {
                        model.TypeId = foodOne.TypeId;
                        model.FoodName = foodOne.FoodName;

                        if (foodOne.Discount > 0)
                        {
                            model.Price = (foodOne.Price - (foodOne.Price * foodOne.Discount) / 100);
                        }
                        else
                        {
                            model.Price = foodOne.Price;
                        }
                        model.NumberOfFood = numberOfFood1;
                        model.QuarterId = quarterId1;


                        if (model.QuarterId == 1)
                        {
                            model.TotalPrice = (numberOfFood1 * foodOne.Price) / 2;
                        }
                        else if (model.QuarterId == 2)
                        {
                            model.TotalPrice = numberOfFood1 * foodOne.Price;
                        }
                        else if (model.QuarterId == 3)
                        {
                            model.TotalPrice = (numberOfFood1 * foodOne.Price) * 2;
                        }

                        int updateAvailable = Convert.ToInt32(foodOne.AvailableInStock) - numberOfFood1;

                        foodOne.AvailableInStock = Convert.ToString(updateAvailable);
                    }

                    if (foodId2 > 0 && numberOfFood2 > 0 && quarterId2 > 0)
                    {
                        model.TypeId2 = foodTwo.TypeId;
                        model.FoodName2 = foodTwo.FoodName;

                        if (foodTwo.Discount > 0)
                        {
                            model.Price2 = (foodTwo.Price - (foodTwo.Price * foodTwo.Discount) / 100);
                        }
                        else
                        {
                            model.Price2 = foodTwo.Price;
                        }
                        model.NumberOfFood2 = numberOfFood2;
                        model.QuarterId2 = quarterId2;


                        if (model.QuarterId2 == 1)
                        {
                            model.TotalPrice2 = (numberOfFood2 * foodTwo.Price) / 2;
                        }
                        else if (model.QuarterId2 == 2)
                        {
                            model.TotalPrice2 = numberOfFood2 * foodTwo.Price;
                        }
                        else if (model.QuarterId2 == 3)
                        {
                            model.TotalPrice2 = (numberOfFood2 * foodTwo.Price) * 2;
                        }


                        int updateAvailable = Convert.ToInt32(foodTwo.AvailableInStock) - numberOfFood2;

                        foodTwo.AvailableInStock = Convert.ToString(updateAvailable);

                    }

                    if (foodId3 > 0 && numberOfFood3 > 0 && quarterId3 > 0)
                    {
                        model.TypeId3 = foodThree.TypeId;
                        model.FoodName3 = foodThree.FoodName;

                        if (foodThree.Discount > 0)
                        {
                            model.Price3 = (foodThree.Price - (foodThree.Price * foodThree.Discount) / 100);
                        }
                        else
                        {
                            model.Price3 = foodThree.Price;
                        }
                        model.NumberOfFood3 = numberOfFood3;
                        model.QuarterId3 = quarterId3;

                        if (model.QuarterId3 == 1)
                        {
                            model.TotalPrice3 = (numberOfFood3 * foodThree.Price)/2;
                        }
                        else if (model.QuarterId3 == 2)
                        {
                            model.TotalPrice3 = numberOfFood3 * foodThree.Price;
                        }
                        else if (model.QuarterId3 == 3)
                        {
                            model.TotalPrice3 = (numberOfFood3 * foodThree.Price) * 2;
                        }


                        int updateAvailable = Convert.ToInt32(foodThree.AvailableInStock) - numberOfFood3;

                        foodThree.AvailableInStock = Convert.ToString(updateAvailable);

                    }

                    int paymentMethod =
                        _context.PaymentMethod.FirstOrDefaultAsync(c => c.PaymentMethodName == "Cash on Delivery").Result.PaymentMethodId;

                    model.SubTotalPrice = model.TotalPrice + model.TotalPrice2 + model.TotalPrice3;
                    model.PaymentMethodId = paymentMethod;
                    model.OrderDate = DateTime.Now;


                    _context.FoodOrder.Add(model);

                    _context.Food.Update(foodOne);
                    _context.Food.Update(foodTwo);
                    _context.Food.Update(foodThree);

                    _context.SaveChanges();

                }
                else
                {
                    return RedirectToAction("Contact", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


            var applicationDbContext = _context.Food.Include(f => f.FoodType);

            ViewBag.foodList = applicationDbContext;
            ViewBag.totalCount = applicationDbContext.Count();

            return View();
        }


        public IActionResult OurTeam()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
