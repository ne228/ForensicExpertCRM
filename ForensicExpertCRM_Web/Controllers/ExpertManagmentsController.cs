using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;

namespace ForensicExpertCRM_Web.Controllers
{
    public class ExpertManagmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpertManagmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExpertManagments
        public async Task<IActionResult> Index()
        {
              return _context.ExpertManagments != null ? 
                          View(await _context.ExpertManagments.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ExpertManagments'  is null.");
        }

        // GET: ExpertManagments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExpertManagments == null)
            {
                return NotFound();
            }

            var expertManagment = await _context.ExpertManagments.Include(x=>x.Experts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expertManagment == null)
            {
                return NotFound();
            }

            return View(expertManagment);
        }

        // GET: ExpertManagments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpertManagments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] ExpertManagment expertManagment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expertManagment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expertManagment);
        }

        // GET: ExpertManagments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExpertManagments == null)
            {
                return NotFound();
            }

            var expertManagment = await _context.ExpertManagments.FindAsync(id);
            if (expertManagment == null)
            {
                return NotFound();
            }
            return View(expertManagment);
        }

        // POST: ExpertManagments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] ExpertManagment expertManagment)
        {
            if (id != expertManagment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expertManagment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpertManagmentExists(expertManagment.Id))
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
            return View(expertManagment);
        }

        // GET: ExpertManagments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExpertManagments == null)
            {
                return NotFound();
            }

            var expertManagment = await _context.ExpertManagments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expertManagment == null)
            {
                return NotFound();
            }

            return View(expertManagment);
        }

        // POST: ExpertManagments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExpertManagments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ExpertManagments'  is null.");
            }
            var expertManagment = await _context.ExpertManagments.FindAsync(id);
            if (expertManagment != null)
            {
                _context.ExpertManagments.Remove(expertManagment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpertManagmentExists(int id)
        {
          return (_context.ExpertManagments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
