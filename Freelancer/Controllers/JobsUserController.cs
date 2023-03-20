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
    [Authorize]
    public class JobsUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsUserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Job.Include(j => j.JobType);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Accept()
        {
            return View();
        }
    }
}
