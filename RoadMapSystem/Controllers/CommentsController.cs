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
    public class CommentsController : Controller
    {
        private readonly RoadMapSystemContext _context;

        public CommentsController(RoadMapSystemContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var roadMapSystemContext = _context.Comment.Include(c => c.MileStone);
            return View(await roadMapSystemContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.MileStone)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(int id, string login)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["MileStoneId"] = new SelectList(_context.MileStone, "MileStoneId", "MileStoneId");

            var Login = await _context.Employee.Include(m => m.EmployeeAccount).FirstOrDefaultAsync(m => m.EmployeeAccount.Login == login);
            var Milestone = await _context.MileStone.Include(m => m.EmployeeSkillValue.Skill).FirstOrDefaultAsync(m => m.MileStoneId == id);
            ViewBag.Name = Login.Name;
            ViewBag.Surname = Login.Surname;
            ViewBag.Id = Milestone.EmployeeSkillValue.Skill.SkillTitle;
            ViewBag.Idd = id;
            ViewBag.IdM = Login.EmployeeId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,AuthorId,MileStoneId,CommentValue")] Comment comment, string Login)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MileStones", new { login = Login });
            }
            ViewData["MileStoneId"] = new SelectList(_context.MileStone, "MileStoneId", "MileStoneId", comment.MileStoneId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["MileStoneId"] = new SelectList(_context.MileStone, "MileStoneId", "MileStoneId", comment.MileStoneId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,AuthorId,MileStoneId,CommentValue")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["MileStoneId"] = new SelectList(_context.MileStone, "MileStoneId", "MileStoneId", comment.MileStoneId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.MileStone)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
