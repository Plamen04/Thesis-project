using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Freelancer.Models;
using Freelancer.Data;

namespace Freelancer.Controllers
{
    public class JobsUserController : Controller
    {
        private readonly FreelancerContext _context;
        public JobsUserController(FreelancerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Job.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
    }
}
