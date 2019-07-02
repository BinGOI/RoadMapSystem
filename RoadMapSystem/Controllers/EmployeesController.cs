using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadMapSystem.Models.DB;
using RoadMapSystem.ViewModels;

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
        //public async Task<IActionResult> Index()
        //{
        //    var roadMapSystemContext = _context.Employee.Include(e => e.EmployeeRole).Include(e => e.Rank);
        //    return View(await roadMapSystemContext.ToListAsync());
        //}

        // GET: Employees/Details/5
        public async Task<IActionResult> Info(string login)
        {
            if (login == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeRole)
                .Include(e => e.Rank)
                .Include(e => e.EmployeeAccount)
                .Include(e => e.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(e => e.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
                .Include(e => e.EmployeeSkillValue).ThenInclude(s => s.Skill).ThenInclude(s => s.SkillValue)

                .FirstOrDefaultAsync(e => e.EmployeeAccount.Login == login);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeLessRang = _context.Employee.Where(e => e.RankId < employee.RankId);
            List<int> tempIdList = employee.EmployeeMentorsMentor.Select(q => q.Intern.EmployeeId).ToList();
            var employeeMaybe = employeeLessRang.Where(q => !tempIdList.Contains(q.EmployeeId));
            var employeeMaybeSelectList =employeeMaybe.Select(e => new
            {
                EmpId = e.EmployeeId,
                Name = e.Name + " " + e.Surname + " " + e.Patronymic
            }).ToList();
            ViewBag.Employee = new SelectList(employeeMaybeSelectList, "EmpId", "Name");
            return View(employee);
        }


        public async Task<IActionResult> SkillCompare(string login)
        {
            if (login == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.EmployeeRole)
                .Include(e => e.Rank)
                .Include(e => e.EmployeeAccount)
                .Include(e => e.EmployeeMentorsIntern).ThenInclude(m => m.Mentor).ThenInclude(m => m.EmployeeAccount)
                .Include(e => e.EmployeeMentorsMentor).ThenInclude(m => m.Intern).ThenInclude(m => m.EmployeeAccount)
                .Include(e => e.EmployeeSkillValue).ThenInclude(s => s.Skill).ThenInclude(s => s.SkillValue)
                .FirstOrDefaultAsync(e => e.EmployeeAccount.Login == login);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeRankSeq =  _context.EmployeeRank.Where(u => u.EmployeeRankId == employee.RankId + 1).Include(u => u.SkillValueRank).ThenInclude(u => u.Skill).ThenInclude(u => u.SkillValue);
            EmployeeRank employeeRank;
            if (employeeRankSeq.Count() <= 0)
            {
                employeeRank = _context.EmployeeRank.Where(u => u.EmployeeRankId == employee.RankId).Include(u => u.SkillValueRank).ThenInclude(u => u.Skill).ThenInclude(u => u.SkillValue).First();
            }
            else
            {
                employeeRank = _context.EmployeeRank.Where(u => u.EmployeeRankId == employee.RankId + 1).Include(u => u.SkillValueRank).ThenInclude(u => u.Skill).ThenInclude(u => u.SkillValue).First();
            }
            if (employeeRank == null)
            {
                return NotFound();
            }
            var ComparerModel = new CompareModel
            {
                employeeProfile = employee,
                compRank = employeeRank
            };
            return View(ComparerModel);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName");
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle");
            return View();
        }


        public IActionResult AddExistingIntern(string MentorLogin, int? InternId)
        {
            if(MentorLogin == null || InternId == null)
            {
                return NotFound();

            }
            var Mentor = _context.Employee.Include(m => m.EmployeeAccount).Where(m=>m.EmployeeAccount.Login == MentorLogin).First();
            var Intern = _context.Employee.Find(InternId);
            if(Mentor == null || Intern == null)
            {
                return NotFound();
            }
            EmployeeMentors employeeMentors = new EmployeeMentors
            {
                MentorId = Mentor.EmployeeId,
                InternId = Intern.EmployeeId,
            
            };
            _context.EmployeeMentors.Add(employeeMentors);
            _context.SaveChangesAsync();
            return RedirectToAction("Info", new { login = Mentor.EmployeeAccount.Login });
        }

        public IActionResult RemoveExistingIntern(string MentorLogin, int? InternId)
        {
            if (MentorLogin == null || InternId == null)
            {
                return NotFound();

            }
            var Mentor = _context.Employee.Include(m => m.EmployeeAccount).Where(m => m.EmployeeAccount.Login == MentorLogin).First();
            var Intern = _context.Employee.Find(InternId);
            if (Mentor == null || Intern == null)
            {
                return NotFound();
            }
            var InternMentor = _context.EmployeeMentors.Include(m => m.Mentor).Where(m => m.Mentor.EmployeeAccount.Login == MentorLogin && m.InternId == InternId);
            _context.EmployeeMentors.RemoveRange(InternMentor);
            _context.SaveChangesAsync();
            return RedirectToAction("Info", new { login = Mentor.EmployeeAccount.Login });

        }

       public IActionResult ChangeRank(string login, int? rankid)
        {
            if (login == null || rankid == null)
            {
                return NotFound();
            }
            var employee = _context.Employee.Include(e => e.EmployeeAccount).Where(m => m.EmployeeAccount.Login == login).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            employee.RankId = rankid.Value;

            _context.Update(employee);
            _context.SaveChangesAsync();
            return RedirectToAction("Info", new { login = login });
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.Include(e => e.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName", employee.EmployeeRoleId);
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle", employee.RankId);
            ViewBag.Login = employee.EmployeeAccount.Login;

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
                string _login = _context.EmployeeAccount.FirstOrDefault(u => u.EmployeeAccountId == employee.EmployeeId).Login;
                return RedirectToAction("Info", new {login = _login});
            }
            ViewData["EmployeeRoleId"] = new SelectList(_context.EmployeeRole, "EmployeeRoleId", "EmployeeRoleName", employee.EmployeeRoleId);
            ViewData["RankId"] = new SelectList(_context.EmployeeRank, "EmployeeRankId", "EmployeeRankTitle", employee.RankId);
            return View(employee);
        }

        
        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
