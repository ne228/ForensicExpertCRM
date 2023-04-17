using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Employee;
using ForensicExpertCRM_Web.Models.Expert;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test_OP_Web;

namespace ForensicExpertCRM_Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EmployeeController(ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

            var experts = await _context.Employees.Include(x=>x.EmployeeManagment)
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
            ViewBag.EmployeeManagments = _context.EmployeeManagments.ToList().EmployeeManagmentToCheckBoxModel();

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
                var entityEmployee = employee.ToEntity();

                var temp = employee.EmployeeManagments.FirstOrDefault(x => x.IsSelected);
                var employeeManagment = _context.EmployeeManagments.Include(x => x.Employees).FirstOrDefault(x => x.Id == temp.Id);

                employeeManagment.Employees.Add(entityEmployee);

                //_context.ExpertManagments.FirstOrDefault();
                var user = await _userManager.CreateUserWithoutEmailConfirm(_roleManager, employee.Login, employee.Password, "user", entityEmployee as Employee);

                if (user == null) new Exception("Ошибка при создании пользователся");


                //var expertUser = user as Expert;
                //expertUser.TypesExpertise = entityExpert.TypesExpertise;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

    }
}
