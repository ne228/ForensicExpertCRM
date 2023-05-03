using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ForensicExpertCRM_Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class TypeExpertisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeExpertisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeExpertises
        public async Task<IActionResult> Index()
        {
              return _context.TypesExpertise != null ? 
                          View(await _context.TypesExpertise.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TypesExpertise'  is null.");
        }

        // GET: TypeExpertises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TypesExpertise == null)
            {
                return NotFound();
            }

            var typeExpertise = await _context.TypesExpertise
                .Include(x=>x.Experts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeExpertise == null)
            {
                return NotFound();
            }

            return View(typeExpertise);
        }

        // GET: TypeExpertises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeExpertises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TypeExpertise typeExpertise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeExpertise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeExpertise);
        }

        // GET: TypeExpertises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TypesExpertise == null)
            {
                return NotFound();
            }

            var typeExpertise = await _context.TypesExpertise.FindAsync(id);
            if (typeExpertise == null)
            {
                return NotFound();
            }
            return View(typeExpertise);
        }

        // POST: TypeExpertises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TypeExpertise typeExpertise)
        {
            if (id != typeExpertise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeExpertise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExpertiseExists(typeExpertise.Id))
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
            return View(typeExpertise);
        }

        // GET: TypeExpertises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TypesExpertise == null)
            {
                return NotFound();
            }

            var typeExpertise = await _context.TypesExpertise
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeExpertise == null)
            {
                return NotFound();
            }

            return View(typeExpertise);
        }

        // POST: TypeExpertises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TypesExpertise == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TypesExpertise'  is null.");
            }
            var typeExpertise = await _context.TypesExpertise.FindAsync(id);
            if (typeExpertise != null)
            {
                _context.TypesExpertise.Remove(typeExpertise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExpertiseExists(int id)
        {
          return (_context.TypesExpertise?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
