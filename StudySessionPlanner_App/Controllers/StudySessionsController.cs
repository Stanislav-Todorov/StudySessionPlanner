using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudySessionPlanner_App.Data;
using StudySessionPlanner_App.Models;
using StudySessionPlanner_App.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySessionPlanner_App.Controllers
{
    public class StudySessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IFeedbackService _feedbackService;


        public StudySessionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEnrollmentService enrollmentService, IFeedbackService feedbackService)
        {
            _context = context;
            _userManager = userManager;
            _enrollmentService = enrollmentService;
            _feedbackService = feedbackService;
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
                .Include(s => s.Feedbacks)
                .ThenInclude(f => f.User)
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

            await _enrollmentService.EnrollUserAsync(id, user.Id);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveFeedback(int id, string comment, int rating)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            await _feedbackService.AddFeedbackAsync(id, user.Id, comment, rating);

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool StudySessionExists(int id)
        {
            return _context.StudySessions.Any(e => e.Id == id);
        }
    }
}
