﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OskarLAspNet.Contexts;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.Identity;

namespace OskarLAspNet.Controllers
{

    public class AdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ProductService _productService;
        private readonly ContactFormService _contactFormService;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(DataContext dataContext, ProductService productService, ContactFormService contactFormService, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _productService = productService;
            _contactFormService = contactFormService;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult GetAllUsers()
        {
            List<AppUser> userList = _dataContext.Users.ToList();
            return View(userList);
            
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminProducts()
        {

            var products = await _productService.GetAllAsync();
            return View(products);

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ShowAllComments()
        {

            var comments = await _contactFormService.GetAllAsync();
            return View(comments);

        }

        //TEST
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            // Retrieve the user based on the provided userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found, handle accordingly
                return RedirectToAction("GetAllUsers");
            }

            // Retrieve the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the current roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Assign the new role
            await _userManager.AddToRoleAsync(user, newRole);

            return RedirectToAction("GetAllUsers");
        }

    }
}
