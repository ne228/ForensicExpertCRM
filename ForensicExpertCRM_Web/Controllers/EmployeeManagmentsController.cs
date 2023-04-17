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
    public class EmployeeManagmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeManagmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeManagments
        public async Task<IActionResult> Index()
        {
            return _context.EmployeeManagments != null ?
                        View(await _context.EmployeeManagments.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.EmployeeManagments'  is null.");
        }

        // GET: EmployeeManagments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeManagments == null)
            {
                return NotFound();
            }

            var employeeManagment = await _context.EmployeeManagments
                .Include(x => x.Employees)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeManagment == null)
            {
                return NotFound();
            }

            return View(employeeManagment);
        }

        // GET: EmployeeManagments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeeManagments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] EmployeeManagment employeeManagment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeManagment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeManagment);
        }

        // GET: EmployeeManagments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmployeeManagments == null)
            {
                return NotFound();
            }

            var employeeManagment = await _context.EmployeeManagments.FindAsync(id);
            if (employeeManagment == null)
            {
                return NotFound();
            }
            return View(employeeManagment);
        }

        // POST: EmployeeManagments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] EmployeeManagment employeeManagment)
        {
            if (id != employeeManagment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeManagment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeManagmentExists(employeeManagment.Id))
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
            return View(employeeManagment);
        }

        // GET: EmployeeManagments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeManagments == null)
            {
                return NotFound();
            }

            var employeeManagment = await _context.EmployeeManagments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeManagment == null)
            {
                return NotFound();
            }

            return View(employeeManagment);
        }

        // POST: EmployeeManagments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeManagments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmployeeManagments'  is null.");
            }
            var employeeManagment = await _context.EmployeeManagments.FindAsync(id);
            if (employeeManagment != null)
            {
                _context.EmployeeManagments.Remove(employeeManagment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeManagmentExists(int id)
        {
            return (_context.EmployeeManagments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
