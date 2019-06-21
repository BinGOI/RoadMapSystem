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

        // GET: MileStones
        public async Task<IActionResult> Index()
        {
            var roadMapSystemContext = _context.MileStone.Include(m => m.EmployeeSkillValue);
            return View(await roadMapSystemContext.ToListAsync());
        }

        // GET: MileStones/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: MileStones/Create
        public IActionResult Create()
        {
            ViewData["EmployeeSkillValueId"] = new SelectList(_context.EmployeeSkillValue, "EmployeeSkillValueId", "EmployeeSkillValueId");
            return View();
        }

        // POST: MileStones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MileStoneId,EmployeeSkillValueId,Date")] MileStone mileStone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mileStone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
    }
}