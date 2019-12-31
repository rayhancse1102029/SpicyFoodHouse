using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpicyFoodHouse.Data;
using SpicyFoodHouse.Models;

namespace AbuRayhanWeb.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index(string srctext, int page)
        {

            ViewBag.srctext = srctext;

            ViewBag.GetAllRoles = _context.Roles;

            ViewBag.TotalCount = _context.Roles.Count();

            return View();
        }

        // Custom Create Role

        public IActionResult CreateRole()
        {
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            string msg = "";
            if (!String.IsNullOrEmpty(rolename))
            {
                var exist = await _roleManager.RoleExistsAsync(rolename);
                if (!exist)
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = rolename });
                    msg = "Role " + rolename + " has been created.";
                }
                else
                {
                    msg = "Role " + rolename + " already exist.";
                }
            }
            ViewBag.msg = msg;
            return View("CreateRole");
        }

        public IActionResult AssignRole()
        {
            var roles = _roleManager.Roles;

            List<string> rolelist = new List<string>();

            foreach (var item in roles)
            {
                rolelist.Add(item.Name);
            }

            ViewBag.roles = rolelist;
            ViewBag.msg = "";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string useremail, string rname)
        {
            string msg = "";
            if (!String.IsNullOrEmpty(useremail))
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(useremail);

                if (!String.IsNullOrEmpty(rname))
                {
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.AddToRoleAsync(user, rname);
                        if (result.Succeeded)
                        {
                            msg = rname + " Role has been assigned to User " + useremail + ".";
                        }
                        else
                        {
                            msg = "Sorry ! Could not assigned role to User " + useremail + ".";

                        }
                    }
                    else
                    {
                        msg = "User " + useremail + " does not exist.";
                    }
                }
                else
                {
                    msg = "Role " + rname + " does not exist.";
                }
            }
            else
            {
                msg = "User email must be entered.";
            }

            ViewBag.msg = msg;

            var roles = _roleManager.Roles;
            List<string> rolelist = new List<string>();
            foreach (var item in roles)
            {
                rolelist.Add(item.Name);
            }
            ViewBag.roles = rolelist;

            return View("AssignRole");
        }


    }
}