using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadMapSystem.Models.DB;

namespace RoadMapSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly RoadMapSystemContext _context;

        public EmployeesController(RoadMapSystemContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var roadMapSystemContext = _context.Employee.Include(e => e.EmployeeRole).Include(e => e.Rank).Include(e => e.EmployeeAccount);
            return View(await roadMapSystemContext.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeRole)
                .Include(e => e.Rank)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName");
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,Surname,Patronymic,Email,PhoneNumber,EmployeeRoleId,RankId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName", employee.EmployeeRoleId);
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle", employee.RankId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName", employee.EmployeeRoleId);
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle", employee.RankId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Surname,Patronymic,Email,PhoneNumber,EmployeeRoleId,RankId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName", employee.EmployeeRoleId);
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle", employee.RankId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeRole)
                .Include(e => e.Rank)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
