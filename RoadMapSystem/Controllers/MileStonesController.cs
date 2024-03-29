﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadMapSystem.Models.DB;

namespace RoadMapSystem.Controllers
{
    public class MileStonesController : Controller
    {
        private readonly RoadMapSystemContext _context;

        public MileStonesController(RoadMapSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexForMentor(string login)
        {
            if (login == null)
            {
                return NotFound();
            }

            var Login = await _context.Employee.Include(m => m.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeAccount.Login == login);

            var roadMapSystemContext = _context.MileStone.Include(m => m.EmployeeSkillValue)
                .Include(m => m.EmployeeSkillValue.Skill)
                .Include(m => m.Comment)
                .Include(m => m.EmployeeSkillValue.Employee)
                .Where(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern.Any(z => z.MentorId == Login.EmployeeId));

            ViewBag.Name = Login.Name;
            ViewBag.Surname = Login.Surname;
            ViewBag.Role = Login.EmployeeRoleId;
            if (roadMapSystemContext == null)
            {
                return NotFound();
            }
            return View(await roadMapSystemContext.ToListAsync());
        }

        // GET: MileStones
        public async Task<IActionResult> Index(string login)
        {
            if (login == null)
            {
                return NotFound();
            }

            //var roadMapSystemContext = _context.MileStone.Include(m => m.EmployeeSkillValue)
            //    .Include(m => m.EmployeeSkillValue.Skill)
            //    .Include(m => m.Comment)
            //    .Include(m => m.EmployeeSkillValue.Employee)
            //    .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
            //    .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
            //    .Where(m => m.EmployeeSkillValue.Employee.EmployeeAccount.Login == login);

            var Login = await _context.Employee.Include(m => m.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeAccount.Login == login);

            var roadMapSystemContext = _context.MileStone.Include(m => m.EmployeeSkillValue)
                .Include(m => m.EmployeeSkillValue.Skill)
                .Include(m => m.Comment)
                .Include(m => m.EmployeeSkillValue.Employee)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeAccount)
                .Where(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern.Any(z => z.MentorId == Login.EmployeeId) || m.EmployeeSkillValue.Employee.EmployeeAccount.Login == login);

            //var Login = await _context.Employee.Include(m => m.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeAccount.Login == login);
            ViewBag.Name = Login.Name;
            ViewBag.Surname = Login.Surname;
            ViewBag.Login = Login.EmployeeAccount.Login;
            ViewBag.Role = Login.EmployeeRoleId;
            if (roadMapSystemContext == null)
            {
                return NotFound();
            }
            return View(await roadMapSystemContext.ToListAsync());
        }

        // GET: MileStones/Details/5
        public async Task<IActionResult> Details(int? id, string login)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Login = await _context.Employee.Include(m => m.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeAccount.Login == login);
            ViewBag.Role = Login.EmployeeRoleId;
            ViewBag.Login = login;
            var mileStone = await _context.MileStone.Include(m => m.EmployeeSkillValue)
                .Include(m => m.EmployeeSkillValue.Skill)
                .Include(m => m.EmployeeSkillValue.Skill.SkillValue)
                .Include(m => m.Comment)
                .Include(m => m.EmployeeSkillValue.Employee)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
                .FirstOrDefaultAsync(m => m.MileStoneId == id);
            if (mileStone == null)
            {
                return NotFound();
            }

            return View(mileStone);
        }

        // GET: MileStones/Create
        public IActionResult Create()
        {
            var SkillList =
                _context.EmployeeSkillValue
                .Select(e => new
                {
                    EmployeeSkillValueId = e.EmployeeSkillValueId,
                    SkillTitle = $"Milestone для {e.Employee.Name} {e.Employee.Surname} - {e.Skill.SkillTitle}" 
                }).Distinct().ToList();
            ViewData["EmployeeSkillValueId"] = new SelectList(SkillList, "EmployeeSkillValueId", "SkillTitle");
            return View();
        }

        // POST: MileStones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MileStoneId,EmployeeSkillValueId,Date")] MileStone mileStone, string Login)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mileStone);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MileStones", new { login = Login });
            }
            ViewData["EmployeeSkillValueId"] = new SelectList(_context.EmployeeSkillValue, "EmployeeSkillValueId", "EmployeeSkillValueId", mileStone.EmployeeSkillValueId);
            return View(mileStone);
        }

        // GET: MileStones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mileStone = await _context.MileStone.FindAsync(id);
            if (mileStone == null)
            {
                return NotFound();
            }
            ViewData["EmployeeSkillValueId"] = new SelectList(_context.EmployeeSkillValue, "EmployeeSkillValueId", "EmployeeSkillValueId", mileStone.EmployeeSkillValueId);
            return View(mileStone);
        }

        // POST: MileStones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MileStoneId,EmployeeSkillValueId,Date")] MileStone mileStone)
        {
            if (id != mileStone.MileStoneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mileStone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MileStoneExists(mileStone.MileStoneId))
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
            ViewData["EmployeeSkillValueId"] = new SelectList(_context.EmployeeSkillValue, "EmployeeSkillValueId", "EmployeeSkillValueId", mileStone.EmployeeSkillValueId);
            return View(mileStone);
        }

        public async Task<IActionResult> EditDate(int id, DateTime date)
        {

              var mileStone = await _context.MileStone.Include(m => m.EmployeeSkillValue)
                .Include(m => m.EmployeeSkillValue.Skill)
                .Include(m => m.EmployeeSkillValue.Skill.SkillValue)
                .Include(m => m.Comment)
                .Include(m => m.EmployeeSkillValue.Employee).ThenInclude(u => u.EmployeeAccount)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(m => m.EmployeeSkillValue.Employee.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
                .FirstOrDefaultAsync(m => m.MileStoneId == id);
            mileStone.Date = date;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mileStone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MileStoneExists(mileStone.MileStoneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { login = mileStone.EmployeeSkillValue.Employee.EmployeeAccount.Login});
            }
            ViewData["EmployeeSkillValueId"] = new SelectList(_context.EmployeeSkillValue, "EmployeeSkillValueId", "EmployeeSkillValueId", mileStone.EmployeeSkillValueId);
            return View(mileStone);
        }

        // GET: MileStones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mileStone = await _context.MileStone
                .Include(m => m.EmployeeSkillValue)
                .FirstOrDefaultAsync(m => m.MileStoneId == id);
            if (mileStone == null)
            {
                return NotFound();
            }

            return View(mileStone);
        }

        // POST: MileStones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mileStone = await _context.MileStone.FindAsync(id);
            _context.MileStone.Remove(mileStone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MileStoneExists(int id)
        {
            return _context.MileStone.Any(e => e.MileStoneId == id);
        }

       /* public async Task<IActionResult> Info()
        {

            var employee = await _context.Employee
                .Include(e => e.EmployeeRole)
                .Include(e => e.Rank)
                .Include(e => e.EmployeeAccount)
                .Include(e => e.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(e => e.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)

                .FirstOrDefaultAsync(e => e.EmployeeAccount.Login == login);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }*/
    }
}
