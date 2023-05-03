using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Test_OP_Web;

namespace ForensicExpertCRM_Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ExpertController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ExpertRepository _expertRepository;
        public ExpertController(ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager, ExpertRepository expertRepository)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _expertRepository = expertRepository;
        }
        public async Task<IActionResult> Index()
        {

            var returnValue = View(await _context.Experts.ToListAsync());
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
                var expertEntity = expert.ToEntity();
                expertEntity.ExpertManagment = await _context.ExpertManagments.FirstOrDefaultAsync(x => x.Id == expert.ExpertManagmentId);
                await _expertRepository.AddExpertAsync(expertEntity , expert.Login, expert.Password);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ExpertManagments = _context.ExpertManagments.ToList().ExpertManagmentToCheckBoxModel();
            ViewBag.TypeExpertise = _context.TypesExpertise.ToList().TypesExpertiseToCheckBoxModel();
            return View(expert);
        }

    }
}
