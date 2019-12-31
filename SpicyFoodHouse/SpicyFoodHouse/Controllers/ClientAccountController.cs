using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;
using SpicyFoodHouse.Models.AccountViewModels;

namespace SpicyFoodHouse.Controllers
{
    [Authorize]
    public class ClientAccountController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ClientAccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> ClientIndex(int page = 1)
        {

            var user = _userManager.GetUserName(User);

            var userInfo = _userManager.Users.Where(u => u.Email == user);

            ViewBag.UserInfo = userInfo;

            IQueryable<FoodOrder> foodOrder = _context.FoodOrder.Where(f => f.CustomerEmail == user);


            if (page <= 0)
            {
                page = 1;
            }

            int pageSize = 10;

            return View(await PaginatedList<FoodOrder>.CreateAsync(foodOrder, page, pageSize));
        }

        [HttpGet]
        public IActionResult UpdateClientAccount()
        {

            var user = _userManager.GetUserName(User);

            ApplicationUser applicationUser = _userManager.Users.FirstOrDefault(u => u.Email == user);

            return View(applicationUser);
            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClientAccount(ApplicationUser registerViewModel, IFormFile nidOrbith, IFormFile profileImage, int page = 1)
        {


            var user = _userManager.GetUserName(User);

            ApplicationUser applicationUser = _userManager.Users.FirstOrDefault(u => u.Email == user);


            if (!string.IsNullOrEmpty(registerViewModel.CustomerName))
            {

                applicationUser.CustomerName = registerViewModel.CustomerName;
            }
            if (!string.IsNullOrEmpty(registerViewModel.Email))
            {

                applicationUser.CustomerName = registerViewModel.Email;
            }


            if (nidOrbith == null)
            {
                applicationUser.NidOrBith = applicationUser.NidOrBith;
            }
            else if (nidOrbith.Length > 0)
            {
                byte[] p1 = null;

                using (var fs1 = nidOrbith.OpenReadStream())
                using (var ms1 = new MemoryStream())
                {
                    fs1.CopyTo(ms1);
                    p1 = ms1.ToArray();

                    applicationUser.NidOrBith = p1;

                }
            }


            if (profileImage == null)
            {
                applicationUser.ProfileImage = applicationUser.ProfileImage;
            }
            else if (profileImage.Length > 0)
            {

                byte[] p2 = null;

                using (var fs2 = profileImage.OpenReadStream())
                using (var ms2 = new MemoryStream())
                {
                    fs2.CopyTo(ms2);
                    p2 = ms2.ToArray();

                    applicationUser.ProfileImage = p2;

                }
            }
            if (!string.IsNullOrEmpty(Convert.ToString(registerViewModel.Phone)))
            {
                applicationUser.Phone = Convert.ToInt32(registerViewModel.Phone);
            }
            if (!string.IsNullOrEmpty(registerViewModel.Address))
            {

                applicationUser.Address = registerViewModel.Address;
            }

            try
            {
                await _userManager.UpdateAsync(applicationUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }


            // Return to Client Index view


            var appUser = _userManager.GetUserName(User);

            var userInfo = _userManager.Users.Where(u => u.Email == appUser);

            ViewBag.UserInfo = applicationUser;

            IQueryable<FoodOrder> foodOrder = _context.FoodOrder.Where(f => f.CustomerEmail == user);


            if (page <= 0)
            {
                page = 1;
            }

            int pageSize = 5;

            return View("ClientIndex",await PaginatedList<FoodOrder>.CreateAsync(foodOrder, page, pageSize));

        }
    }
}