using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadMapSystem.Models;
using RoadMapSystem.Models.DB;
using RoadMapSystem.ViewModels;

namespace RoadMapSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoadMapSystemContext _context;

        public AccountController(RoadMapSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult NotPermission()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = await _context.Employee.Include(u => u.EmployeeAccount).FirstOrDefaultAsync(u => u.EmployeeAccount.Login == model.Login && u.EmployeeAccount.Password == model.Password);
                if (employee != null)
                {
                    await Authenticate(employee); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(Employee employee)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.EmployeeAccount.Login),
                //new Claim(ClaimsIdentity.DefaultRoleClaimType, employee.EmployeeType.EmployyeTypeDescription),
                new Claim("Namee", employee.Name),
                new Claim("Surname", employee.Surname),
                new Claim("Patronymic", employee.Patronymic)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}