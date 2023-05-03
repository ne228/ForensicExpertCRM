using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Employee;
using ForensicExpertCRM_Web.Models.Expert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForensicExpertCRM_Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmployeeRepository employeeRepository;
        public EmployeeController(ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager, EmployeeRepository employeeRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            this.employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Index()
        {

            var returnValue = View(await _context.Employees.ToListAsync());

            return returnValue;
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var experts = await _context.Employees.Include(x => x.EmployeeManagment)
            .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (experts == null)
            {
                return NotFound();
            }

            return View(experts);
        }


        // GET: ExpertManagments/Create
        public IActionResult Create()
        {
            
            ViewBag.EmployeeManagments = _context.EmployeeManagments.ToList();
            return View();
        }

        // POST: ExpertManagments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                await employeeRepository.CreateAsync(employee.ToEntity(), employee.Login, employee.Password);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.EmployeeManagments = _context.EmployeeManagments.ToList();
            return View(employee);
        }

    }
}
