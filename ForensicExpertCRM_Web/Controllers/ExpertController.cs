using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test_OP_Web;

namespace ForensicExpertCRM_Web.Controllers
{
    public class ExpertController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ExpertController(ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {

            var returnValue = View(await _context.Experts.ToListAsync());
            //:
            //            Problem("Entity set 'ApplicationDbContext.ExpertManagments'  is null.");

            return returnValue;
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Experts == null)
            {
                return NotFound();
            }

            var experts = await _context.Experts
                .Include(x => x.ExpertManagment)
                .Include(x => x.TypesExpertise)
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
            ViewBag.ExpertManagments = _context.ExpertManagments.ToList().ExpertManagmentToCheckBoxModel();
            ViewBag.TypeExpertise = _context.TypesExpertise.ToList().TypesExpertiseToCheckBoxModel();
            return View();
        }

        // POST: ExpertManagments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpertViewModel expert)
        {
            if (ModelState.IsValid)
            {

                // Конвертация из модели в сущность
                var entityExpert = expert.ToEntity();

                entityExpert.TypesExpertise = null;

                var expertManagment = expert.ExpertManagments.FirstOrDefault(x => x.IsSelected);
                var ExpertManagment = _context.ExpertManagments.Include(x => x.Experts).FirstOrDefault(x => x.Id == expertManagment.Id);

                ExpertManagment.Experts.Add(entityExpert);
                
                var res = await _userManager.CreateUserWithoutEmailConfirm(_roleManager, expert.Login, expert.Password, "employee", entityExpert) as Expert;
                if (res == null) throw new Exception("Ошибка при создании пользователся");
               
                //res = res as Expert;
                var temp = expert.ToEntity();

                var user = _context.Experts
                    .Include(x => x.TypesExpertise)
                    .FirstOrDefault(x => x.Id == res.Id);


                user.TypesExpertise.AddRange(temp.TypesExpertise);
                //res.ExpertManagment = temp.ExpertManagment;
                // Получение управления эксперта



                await _context.SaveChangesAsync();
             
                return RedirectToAction(nameof(Index));
            }
            return View(expert);
        }




    }
}
