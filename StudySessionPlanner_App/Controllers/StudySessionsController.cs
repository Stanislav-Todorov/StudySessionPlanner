using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace StudySessionPlanner_App.Controllers
{
    public class StudySessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public StudySessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StudySessions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StudySessions.Include(s => s.Topic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudySessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySession = await _context.StudySessions
                .Include(s => s.Topic)
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studySession == null)
            {
                return NotFound();
            }

            return View(studySession);
        }

        // GET: StudySessions/Create
        public IActionResult Create()
        {
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name");
            return View();
        }

        // POST: StudySessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Location,StartTime,DurationMinutes,TopicId")] StudySession studySession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studySession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", studySession.TopicId);
            return View(studySession);
        }

        // GET: StudySessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySession = await _context.StudySessions.FindAsync(id);
            if (studySession == null)
            {
                return NotFound();
            }
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", studySession.TopicId);
            return View(studySession);
        }

        // POST: StudySessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Location,StartTime,DurationMinutes,TopicId")] StudySession studySession)
        {
            if (id != studySession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studySession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudySessionExists(studySession.Id))
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
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", studySession.TopicId);
            return View(studySession);
        }

        // GET: StudySessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studySession = await _context.StudySessions
                .Include(s => s.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studySession == null)
            {
                return NotFound();
            }

            return View(studySession);
        }

        // POST: StudySessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studySession = await _context.StudySessions.FindAsync(id);
            if (studySession != null)
            {
                _context.StudySessions.Remove(studySession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var studySessionExists = await _context.StudySessions.AnyAsync(s => s.Id == id);

            if (!studySessionExists)
            {
                return NotFound();
            }

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudySessionId == id && e.UserId == user.Id);

            if (!alreadyEnrolled)
            {
                var enrollment = new Enrollment
                {
                    StudySessionId = id,
                    UserId = user.Id,
                    JoinedOn = DateTime.UtcNow
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool StudySessionExists(int id)
        {
            return _context.StudySessions.Any(e => e.Id == id);
        }
    }
}
