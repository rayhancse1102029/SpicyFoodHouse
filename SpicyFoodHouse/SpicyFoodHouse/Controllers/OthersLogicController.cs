using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Remotion.Linq.Clauses;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;
using SpicyFoodHouse;

namespace SpicyFoodHouse.Controllers
{
    public class OthersLogicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OthersLogicController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SingleFoodOrder(int id)
        {

            Food foods = await _context.Food.Include(f => f.FoodType).SingleOrDefaultAsync(item => item.FoodId == id);


            IQueryable<FoodQuarter> foodQuarter =  _context.FoodQuarter;

            ViewBag.quarterList = foodQuarter;

            DeliveryCharge deliveryCharge =
                await _context.DeliveryCharge.SingleOrDefaultAsync(f => f.LocationRange == "Cumilla Sadar");

            ViewBag.deliveryCharge = deliveryCharge.Charge;

            // for order

            // customer email from sign in manager
            ViewBag.fooTypeId = foods.TypeId;
            // food name in the above
            // food price in the above
            // number of food from the view page 
            // food Quarter id from the view page
            // total price count in the controller and also calculate in the view page and show in the label



            ViewBag.foodTypes = foods.FoodType.TypeName;
            ViewBag.foodName = foods.FoodName;
            ViewBag.image = foods.Image;
            ViewBag.price = foods.Price;
            ViewBag.discount = foods.Discount;
            ViewBag.availableInStock = Convert.ToInt32(foods.AvailableInStock);
            ViewBag.description = foods.Description;
           
            ViewBag.foodId = foods.FoodId;


            if (foods.IsPopular == true)
            {
                ViewBag.popular = "Popular";
            }

            if (foods.IsTranding == true)
            {
                ViewBag.tranding = "Trending";
            }

            if (foods.IsDiscounted == true)
            {
                ViewBag.discounted = "Discounted";
            }

            
            return View();
        }

        [HttpPost]
        public JsonResult SingleFoodOrder(FoodOrder model)
        {

            string result = "nothing";

            if (_signInManager.IsSignedIn(User))
            {
                FoodOrder foodOrder = new FoodOrder();

                foodOrder.CustomerEmail = _userManager.GetUserName(User);



                Food food = new Food();

                food = _context.Food.FirstOrDefault(n => n.FoodName == model.FoodName);

                int available = Convert.ToInt32(food.AvailableInStock);


                if (available <= 0)
                {
                    result = "notAvailable";
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.TypeId.ToString()) && !string.IsNullOrEmpty(model.FoodName) &&
                        !string.IsNullOrEmpty(model.Price.ToString()) &&
                        !string.IsNullOrEmpty(model.NumberOfFood.ToString()) &&
                        !string.IsNullOrEmpty(model.QuarterId.ToString()) &&
                        !string.IsNullOrEmpty(foodOrder.CustomerEmail))
                    {

                        int paymentMethod =
                            _context.PaymentMethod.FirstOrDefaultAsync(c => c.PaymentMethodName == "Cash on Delivery")
                                .Result.PaymentMethodId;


                        foodOrder.TypeId = model.TypeId;
                        foodOrder.FoodName = model.FoodName;
                        foodOrder.Price = model.Price;
                        foodOrder.NumberOfFood = model.NumberOfFood;
                        foodOrder.QuarterId = model.QuarterId;

                        if (model.QuarterId == 1)
                        {
                            foodOrder.TotalPrice = ((model.Price * model.NumberOfFood) / 2);
                        }
                        else if (model.QuarterId == 2)
                        {
                            foodOrder.TotalPrice = (model.Price * model.NumberOfFood);
                        }
                        else if (model.QuarterId == 3)
                        {
                            foodOrder.TotalPrice = ((model.Price * model.NumberOfFood) * 2);
                        }

                        foodOrder.TypeId2 = 0;
                        foodOrder.FoodName2 = "0";
                        foodOrder.Price2 = 0;
                        foodOrder.NumberOfFood2 = 0;
                        foodOrder.QuarterId2 = 0;
                        foodOrder.TotalPrice2 = 0;

                        foodOrder.TypeId3 = 0;
                        foodOrder.FoodName3 = "0";
                        foodOrder.Price3 = 0;
                        foodOrder.NumberOfFood3 = 0;
                        foodOrder.QuarterId3 = 0;
                        foodOrder.TotalPrice3 = 0;

                        foodOrder.SubTotalPrice = model.Price;
                        foodOrder.PaymentMethodId = paymentMethod;

                        foodOrder.OrderDate = DateTime.Now;


                        int updateAvailable = available - model.NumberOfFood;

                        food.AvailableInStock = updateAvailable.ToString();


                        _context.FoodOrder.Add(foodOrder);

                        _context.Food.Update(food);
                        _context.SaveChanges();

                        result = "success";
                    }
                    else
                    {
                        result = "unvalid";


                    }

                }


            }
            else
            {
                result = "unauthorize";
            }


                return Json(result);
        }
        

        public async Task<IActionResult> ViewAllFood(int id, int page = 1)
        {

            IQueryable<Food> foods = _context.Food;

            string typeSelected = "";

            if (id == 1)
            {
                foods = from item in _context.Food
                    orderby item.FoodName ascending
                    where item.IsTranding == true
                    select item;

                typeSelected = "TRENDING PRODUCTS";
            }
            else if (id == 2)
            {
                foods = from item in _context.Food
                    orderby item.FoodName ascending
                    where item.IsPopular == true
                    select item;

                typeSelected = "POPULAR PRODUCTS";
            }
            else if (id == 3)
            {
                foods = from item in _context.Food
                    orderby item.FoodName ascending
                    where item.IsDiscounted == true
                    select item;

                typeSelected = "DISCOUNTED PRODUCTS";

            }

            ViewBag.totalResultFound = foods.Count();
            ViewBag.typeSelected = typeSelected;

            if (page <= 0) { page = 1; }
            int pageSize = 10;

            return View(await PaginatedList<Food>.CreateAsync(foods, page, pageSize));
        }

        //  Releted food show from the food Deatils page [bottom] same as the ViewAllFood
        
        public async Task<IActionResult> RelatedFood(int id, int page = 1)
        {

            Food foods = await _context.Food.Include(f => f.FoodType).SingleOrDefaultAsync(item => item.FoodId == id);

           IQueryable<Food> relatedFoods = from item in _context.Food
                where item.TypeId == foods.TypeId
                select item;


            ViewBag.relatedFoodTypeId = foods.TypeId;
            ViewBag.totalResultFound = relatedFoods.Count();
            ViewBag.typeSelected = foods.FoodType.TypeName;

            if (page <= 0)
            {
                page = 1;
            }
            int pageSize = 10;

            return View("ViewAllFood", await PaginatedList<Food>.CreateAsync(relatedFoods, page, pageSize));
        }


        public async Task<IActionResult> ViewFoodDetails(int id, int page = 1)
        {

            Food foods = await _context.Food.Include(f => f.FoodType).SingleOrDefaultAsync(item => item.FoodId == id);

            ViewBag.foodName = foods.FoodName;
            ViewBag.image = foods.Image;
            ViewBag.price = foods.Price;
            ViewBag.discount = foods.Discount;
            ViewBag.availableInStock = Convert.ToInt32(foods.AvailableInStock);
            ViewBag.description = foods.Description;
            ViewBag.foodTypes = foods.FoodType.TypeName;
            ViewBag.foodId = foods.FoodId;


            if (foods.IsPopular == true)
            {
                ViewBag.popular = "Popular";
            }

            if (foods.IsTranding == true)
            {
                ViewBag.tranding = "Trending";
            }

            if (foods.IsDiscounted == true)
            {
                ViewBag.discounted = "Discounted";
            }

            IQueryable<Food> relatedFoods = from item in _context.Food
                where item.TypeId == foods.TypeId
                select item;


            ViewBag.relatedFoods = relatedFoods;

            ViewBag.foodType = foods.FoodType.TypeName;
            //ViewBag.typeSelected = foods.TypeId;
            ViewBag.relatedFoodTypeId = foods.TypeId;

            IQueryable < Comment > comment = _context.Comment;

            ViewBag.totalComment = comment.Count();
            if (page <= 0) { page = 1; }
            int pageSize = 10;

            return View(await PaginatedList<Comment>.CreateAsync(comment, page, pageSize));
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(string commentText)
        {

            Comment comment = new Comment();

            if (!string.IsNullOrEmpty(commentText))
            {

                var userName = _userManager.GetUserName(User);

                ApplicationUser applicationUser =  await _userManager.FindByEmailAsync(userName);


                comment.Email = applicationUser.Email;
                comment.Username = applicationUser.CustomerName;
                comment.CommentText = commentText;
                comment.CommentTime = DateTime.Now;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult UpdateAvailableInStocks()
        {



            foreach (var item in _context.AvailableInStock)
            {
                foreach (var food in _context.Food)
                {
                    if (item.FoodName == food.FoodName)
                    {
                        food.AvailableInStock = Convert.ToString(item.AvailableFood);
                        _context.Food.Update(food);
                    }
                }
            }

            _context.SaveChanges();

          
            return RedirectToAction("Index", "Foods");
        }

    }
}