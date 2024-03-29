﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Register(string mentorlogin)
        {
            ViewBag.EmployeeRoleId = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName");
            ViewBag.RankId = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle");
            var Skills = _context.Skill.Select(u => u.SkillTitle).Distinct();
            ViewBag.SkillValueId= new SelectList(_context.SkillValue, "SkillValueId", "SkillValueTitle");
            ViewBag.SkillList = Skills;
            if(!(mentorlogin == null || mentorlogin == ""))
            {
                int mentorId = _context.EmployeeAccount.FirstOrDefault(u => u.Login == mentorlogin).EmployeeAccountId;
                ViewBag.MentorId = mentorId;
            }
            else
            {
                ViewBag.MentorId = "";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                EmployeeAccount employeeAccount = await _context.EmployeeAccount.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (employeeAccount == null)
                {

                    employeeAccount = new EmployeeAccount
                    {
                        Login = model.Login,
                        Password = model.Password
                    };
                    _context.EmployeeAccount.Add(employeeAccount);
                    await _context.SaveChangesAsync();
                    Employee employee = new Employee
                    {
                        EmployeeId = employeeAccount.EmployeeAccountId,
                        Name = model.Name,
                        Surname = model.Surname,
                        Patronymic = model.Patronymic,
                        Email = model.Email,
                        EmployeeRoleId = model.EmployeeRoleId,
                        RankId = model.RankId,
                        PhoneNumber = model.PhoneNumber

                    };
                    _context.Employee.Add(employee);
                    await _context.SaveChangesAsync();
                    if (model.MentorId != null)
                    {
                        EmployeeMentors employeeMentors = new EmployeeMentors
                        {
                            MentorId = model.MentorId.Value,
                            InternId = employee.EmployeeId,
                
                        };
                        _context.EmployeeMentors.Add(employeeMentors);
                    }

                    foreach(var skillVal in model.SkillsValue)
                    {
                        EmployeeSkillValue employeeSkillValue = new EmployeeSkillValue
                        {
                            EmployeeId = employee.EmployeeId,
                            SkillId = _context.Skill.Where(u => u.SkillTitle == skillVal.Key && u.SkillValueId == skillVal.Value).First().IdSkill
                        };
                        _context.EmployeeSkillValue.Add(employeeSkillValue);
                    }
                   await _context.SaveChangesAsync();
                   // await Login(new LoginModel() { Login = model.Login, Password = model.Password}); // аутентификация
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError("", "Такий логін вже використано");
            }
            ViewBag.EmployeeRoleId = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName");
            ViewBag.RankId = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle");
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = await _context.Employee.Include(u => u.EmployeeAccount).Include(u=> u.EmployeeRole).FirstOrDefaultAsync(u => u.EmployeeAccount.Login == model.Login && u.EmployeeAccount.Password == model.Password);
                if (employee != null)
                {
                    await Authenticate(employee); 

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
                new Claim(ClaimsIdentity.DefaultRoleClaimType, employee.EmployeeRole.EmployeeRoleName),
                new Claim("Namee", employee.Name),
                new Claim("Surname", employee.Surname),
                new Claim("Patronymic", employee.Patronymic),
                
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