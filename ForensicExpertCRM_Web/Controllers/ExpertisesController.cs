using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;
using ForensicExpertCRM_Web.Models.Expertise;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Test_OP_Web;

namespace ForensicExpertCRM_Web.Controllers
{
    public class ExpertisesController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ExpertisesController(ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {

            var returnValue = View(await _context.Expertises
                .Include(x => x.TypeExpertise)
                .Include(x => x.Employee)
                .Include(x => x.Expert)
                .ToListAsync());

            return returnValue;
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expertises == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertises
                .Include(x => x.TypeExpertise)
                .Include(x => x.Employee)
                .Include(x => x.Expert)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (expertise == null)
            {
                return NotFound();
            }

            return View(expertise);
        }


        public IActionResult Create()
        {

            ViewBag.TypesExpertise = _context.TypesExpertise.ToList();
            return View();
        }

        // POST: ExpertManagments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpertiseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var expertise = new Expertise()
                {
                    Questions = model.Questions,
                    TermDateTime = model.TermDateTime,
                    CreateDateTime = DateTime.Now,

                };
                var inputTypeExpertise = _context.TypesExpertise.FirstOrDefault(x => x.Id == model.TypeExpertiseId);
                if (inputTypeExpertise == null) { throw new Exception("Неверный id TypeExpertise"); }
                expertise.TypeExpertise = inputTypeExpertise;

                
                // Set employee
                var employee = _userManager.GetUserAsync(HttpContext.User).Result as Employee;

                if (employee == null)
                    throw new Exception("Неверный сотрудник");

                if (!_userManager.GetRolesAsync(employee).Result.Contains("employee"))
                    throw new Exception("Неверная роль пользователя");

                expertise.Employee = employee;


                // Set expert
                var expert = await _context.Experts.FirstOrDefaultAsync(x => x.Id == model.ExpertId);

                if (employee == null)
                    throw new Exception("Не установлен эксперт");
                expertise.Expert = expert;


                //_context.Add(expertise);


                return View(nameof(Accept), expertise);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private List<Expert> getExperts(int typeExpertiseId, string employeeId)
        {

            var res = _context.Experts
              .Where(x => x.TypesExpertise.Any(x => x.Id == typeExpertiseId))
              .OrderByDescending(x => x.Rating)
              .ToList();
            return res;
        }
        public async Task<string> GetExperts(int typeExpertiseId)
        {
            var employee = _userManager.GetUserAsync(HttpContext.User).Result as Employee;
            if (employee == null) return "Авторизуйстесь для создания экспертизы";



            var experts = getExperts(typeExpertiseId, employee.Id);

            var json = JsonConvert.SerializeObject(experts);

            return json;
        }

        public IActionResult Accept()
        {


            ViewBag.Experts = _context.Experts.ToList();
            return View();
        }
    }
}
