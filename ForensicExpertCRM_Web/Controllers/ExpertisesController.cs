using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;
using ForensicExpertCRM_Web.Models.Expertise;
using ForensicExpertCRM_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Test_OP_Web;

namespace ForensicExpertCRM_Web.Controllers
{
    [Authorize]
    public class ExpertisesController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Logic _logic;
        private IWebHostEnvironment _appEnvironment;

        private async Task<Expertise> GetExpertiseAsync(int? expertiseId)
        {
            var user = await _userManager.GetUserAsync(User);



            Expertise expertise = null;
            if (User.IsInRole("admin"))
            {
                expertise = await _context.Expertises
                    .Include(x => x.TypeExpertise)
                    .Include(x => x.Files)
                    .Include(x => x.ExpertFiles)
                    .Include(x => x.Employee).ThenInclude(x => x.EmployeeManagment)
                    .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
                    .FirstOrDefaultAsync(m => m.Id == expertiseId);
            }

            if (user is Expert)
            {
                expertise = await _context.Expertises
                .Include(x => x.TypeExpertise)
                .Include(x => x.Files)
                .Include(x => x.ExpertFiles)
                .Include(x => x.Employee).ThenInclude(x => x.EmployeeManagment)
                .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
                .FirstOrDefaultAsync(m => m.Id == expertiseId && m.Expert == user);
            }

            if (user is Employee)
            {
                expertise = await _context.Expertises
               .Include(x => x.TypeExpertise)
               .Include(x => x.Files)
               .Include(x => x.ExpertFiles)
               .Include(x => x.Employee).ThenInclude(x => x.EmployeeManagment)
               .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
               .FirstOrDefaultAsync(m => m.Id == expertiseId && m.Employee == user);
            }



            return expertise;
        }

        public ExpertisesController(
            ApplicationDbContext context, UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment appEnvironment, Logic logic)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _appEnvironment = appEnvironment;
            _logic = logic;
        }
        public async Task<IActionResult> Index()
        {
                      

            List<Expertise> expertises = new List<Expertise>();


            var user = await _userManager.GetUserAsync(User);
            if (HttpContext.User.IsInRole("admin"))
                expertises = await _context.Expertises
                    .Include(x => x.TypeExpertise)
                    .Include(x => x.Employee)
                    .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
                    .ToListAsync();



            if (HttpContext.User.IsInRole("employee"))
                expertises = await _context.Expertises
                    .Include(x => x.TypeExpertise)
                    .Include(x => x.Employee)
                    .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
                    .Where(x => x.Employee == user)
                    .ToListAsync();
            else if (HttpContext.User.IsInRole("expert"))
                expertises = await _context.Expertises
                   .Include(x => x.TypeExpertise)
                   .Include(x => x.Employee)
                   .Include(x => x.Expert).ThenInclude(x => x.ExpertManagment)
                   .Where(x => x.Expert.Id == user.Id)
                   .ToListAsync();


            expertises = expertises.OrderByDescending(x => x.CreateDateTime).OrderBy(x => x.Accept).ToList();

            return View(expertises);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expertises == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            Expertise expertise = await GetExpertiseAsync(id);

            if (expertise == null)
                return NotFound();


            return View(expertise);
        }

        [Authorize(Roles = "employee")]
        public IActionResult Create()
        {

            ViewBag.TypesExpertise = _context.TypesExpertise.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> Create(ExpertiseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var expertise = new Expertise()
                {
                    //Questions = model.Questions,
                    TermDateTime = (DateTime)model.TermDateTime,
                    CreateDateTime = DateTime.Now,
                    Files = new()
                };
                var inputTypeExpertise = _context.TypesExpertise.FirstOrDefault(x => x.Id == model.TypeExpertiseId);
                if (inputTypeExpertise == null) { throw new Exception("Неверный id TypeExpertise"); }
                expertise.TypeExpertise = inputTypeExpertise;


                // Set employee
                var employee = _userManager.GetUserAsync(HttpContext.User).Result as Employee;
                if (employee == null)
                    throw new Exception("Неверный сотрудник");

                var role = _userManager.GetRolesAsync(employee).Result;
                if (!role.Contains("employee"))
                    throw new Exception("Неверная роль пользователя");

                expertise.Employee = employee;


                // Set expert
                var expert = await _context.Experts.FirstOrDefaultAsync(x => x.Id == model.ExpertId);

                if (employee == null)
                    throw new Exception("Не установлен эксперт");
                expertise.Expert = expert;


                // AddFiles

                string directory = Path.Combine(_appEnvironment.WebRootPath, "files");
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                foreach (var file in model.uploads)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(directory, Guid.NewGuid().ToString().Substring(6) + $".{Path.GetExtension(fileName)}");

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        expertise.Files.Add(new MyFile()
                        {
                            Path = path,
                            FileName = fileName
                        });

                    }
                }

                _context.Add(expertise);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TypesExpertise = _context.TypesExpertise.ToList();
            return View(model);
        }

        private async Task<List<ExpertOut>> getExperts(int typeExpertiseId, string employeeId)
        {
            var res = await _logic.GetExperts(typeExpertiseId, employeeId);
            return res;
        }
        public async Task<string> GetExperts(int typeExpertiseId)
        {
            var employee = _userManager.GetUserAsync(HttpContext.User).Result as Employee;
            if (employee == null) return "Авторизуйтесь для создания экспертизы";


            var experts = await getExperts(typeExpertiseId, employee.Id);
            var json = JsonConvert.SerializeObject(experts);
            return json;
        }

        [Authorize(Roles = "employee")]
        public async Task<IActionResult> AcceptRating(AcceptRatingViewModel model)
        {
            if (ModelState.IsValid)
            {

                var expertie = await GetExpertiseAsync(model.ExpertiseId);

                if (expertie == null) return NotFound();


                if (expertie.AcceptRating)
                    throw new Exception("Рэйтинг нельзя изменить");

                expertie.Rating = model.Rating;
                expertie.AcceptRating = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = model.ExpertiseId });

        }

        [Authorize(Roles = "expert")]

        public async Task<IActionResult> AcceptExpertise(AcceptExpertise model)
        {
            if (ModelState.IsValid)
            {

                var expertise = await GetExpertiseAsync(model.ExpertiseId);
                if (expertise == null) return NotFound();
                expertise.ExpertFiles = new();

                // AddFiles

                string directory = Path.Combine(_appEnvironment.WebRootPath, "files");
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                foreach (var file in model.uploads)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(directory, Guid.NewGuid().ToString().Substring(6) + $".{Path.GetExtension(fileName)}");

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        expertise.ExpertFiles.Add(new()
                        {
                            Path = path,
                            FileName = fileName
                        });

                    }
                }


                expertise.Accept = true;
                //expertise.Answers = model.Answers;
                expertise.AccpetDateTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = model.ExpertiseId });
        }

        public async Task<IActionResult> Download(int id)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == id);

            if (file == null) return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(file.Path);
            return File(fileBytes, "application/octet-stream", file.FileName);
        }
    }
}
