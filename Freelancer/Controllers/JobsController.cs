using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Freelancer.Data;
using Freelancer.Models;
using Microsoft.AspNetCore.Authorization;

namespace Freelancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumer, string currentFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageNumer = 1;
            }
            else searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;
            var jobs = from j in _context.Job.Include(j => j.JobType) select j;
            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j => j.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    jobs = jobs.OrderBy(j => j.Title);
                    break;
                case "Date":
                    jobs = jobs.OrderBy(j => j.CreatedDate);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(j => j.CreatedDate);
                    break;
                default:
                    jobs = jobs.OrderByDescending(j => j.Title);
                    break;
            }
            int pageSize = 20;
            return View(await PaginatedList<Job>.CreateAsync(jobs.AsNoTracking(), pageNumer ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        public IActionResult Create()
        {
            ViewData["JobTypeId"] = new SelectList(_context.jobTypes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,JobTypeId,CreatedDate")] Job job)
        {
            if (ModelState.IsValid)
            {
                job.CreatedDate = DateTime.Now;
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobTypeId"] = new SelectList(_context.jobTypes, "Id", "Name", job.JobTypeId);
            return View(job);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["JobTypeId"] = new SelectList(_context.jobTypes, "Id", "Name", job.JobTypeId);
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,JobTypeId,CreatedDate")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    job.CreatedDate = DateTime.Now;
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["JobTypeId"] = new SelectList(_context.jobTypes, "Id", "Name", job.JobTypeId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.JobType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Job'  is null.");
            }
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                _context.Job.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
          return _context.Job.Any(e => e.Id == id);
        }
    }
}
