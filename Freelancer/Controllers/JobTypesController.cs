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
using Microsoft.Data.SqlClient;

namespace Freelancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobTypesController(ApplicationDbContext context)
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
            var jobs = from j in _context.jobTypes select j;
            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j => j.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    jobs = jobs.OrderBy(j => j.Name);
                    break;
                case "Date":
                    jobs = jobs.OrderBy(j => j.CreatedDate);
                    break;
                case "date_desc":
                    jobs = jobs.OrderByDescending(j => j.CreatedDate);
                    break;
                default:
                    jobs = jobs.OrderByDescending(j => j.Name);
                    break;
            }
            int pageSize = 20;
            return View(await PaginatedList<JobType>.CreateAsync(jobs.AsNoTracking(), pageNumer ?? 1, pageSize));
        }

        // GET: JobTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.jobTypes == null)
            {
                return NotFound();
            }

            var jobType = await _context.jobTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // GET: JobTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreatedDate")] JobType jobType)
        {
            if (ModelState.IsValid)
            {
                jobType.CreatedDate = DateTime.Now;
                _context.Add(jobType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobType);
        }

        // GET: JobTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.jobTypes == null)
            {
                return NotFound();
            }

            var jobType = await _context.jobTypes.FindAsync(id);
            if (jobType == null)
            {
                return NotFound();
            }
            return View(jobType);
        }

        // POST: JobTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreatedDate")] JobType jobType)
        {
            if (id != jobType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    jobType.CreatedDate = DateTime.Now;
                    _context.Update(jobType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobTypeExists(jobType.Id))
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
            return View(jobType);
        }

        // GET: JobTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.jobTypes == null)
            {
                return NotFound();
            }

            var jobType = await _context.jobTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }

        // POST: JobTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.jobTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.jobTypes'  is null.");
            }
            var jobType = await _context.jobTypes.FindAsync(id);
            if (jobType != null)
            {
                _context.jobTypes.Remove(jobType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobTypeExists(int id)
        {
          return _context.jobTypes.Any(e => e.Id == id);
        }
    }
}
