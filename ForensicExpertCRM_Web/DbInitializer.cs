using ForensicExpertCRM_Web.Controllers;
using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;
using ForensicExpertCRM_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ForensicExpertCRM_Web
{
    public static class DbInitializer
    {

        private const int CountEmployee = 10;
        private const int CountExpert = 10;
        private const int CountExpertise = 300;
        public async static Task Initialize(IServiceScope scope, ApplicationDbContext context)
        {
            //context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated())
            {
                await RoleInitialize(scope);
                await CreateExperts(scope, context);

                await context.SaveChangesAsync();
            }
        }


        private static async Task CreateExperts(IServiceScope scope, ApplicationDbContext context)
        {
            var serviceProvider = scope.ServiceProvider;
            var rnd = new Random();


            // Create ExpertMangments
            var EmployeeManagment1 = new EmployeeManagment() { Name = "УМВД России по Адмиралтейскому району города Санкт-Петербурга", Address = "Советский переулок, 9/34, Санкт-Петербург" };
            var EmployeeManagment2 = new EmployeeManagment() { Name = "УМВД России по Фрунзенскому району г. Санкт-Петербурга", Address = "Расстанная улица, 15, Санкт-Петербург" };
            var EmployeeManagment3 = new EmployeeManagment() { Name = "Главное управление Министерства внутренних дел Российской Федерации по городу Санкт-Петербургу и Ленинградской области", Address = "Суворовский проспект, 50-52, Санкт-Петербург" };
            var EmployeeManagment4 = new EmployeeManagment() { Name = "УМВД России по Петроградскому району города Санкт-Петербурга", Address = "Большая Монетная улица, 20, Санкт-Петербург" };
            context.Add(EmployeeManagment1);
            context.Add(EmployeeManagment2);
            context.Add(EmployeeManagment3);
            context.Add(EmployeeManagment4);


            // Create EmployeeManagment
            var ExpertManagment1 = new ExpertManagment() { Name = "Экспертно-криминалистический центр", Address = "ул. Трефолева, 42, Санкт-Петербург" };
            var ExpertManagment2 = new ExpertManagment() { Name = "ЭКЦ ГУВД 14 отдел по городу Санкт-Петербургу и Ленинградской области", Address = "Софийская улица, 8к2, Санкт-Петербург" };
            var ExpertManagment3 = new ExpertManagment() { Name = "27 отдел Экспертно-криминалистического центра", Address = "Московский проспект, 95, Санкт-Петербург" };


            context.Add(ExpertManagment1);
            context.Add(ExpertManagment2);
            context.Add(ExpertManagment3);


            // Create Type of Expertise
            var type1 = new TypeExpertise() { Name = "Автороведческая" };
            var type2 = new TypeExpertise() { Name = "Автотехническая" };
            var type3 = new TypeExpertise() { Name = "Баллистическая" };
            var type4 = new TypeExpertise() { Name = "Биологическая экспертиза тканей и выделений человека, животных" };
            var type5 = new TypeExpertise() { Name = "Дактилоскопическая" };
            var type6 = new TypeExpertise() { Name = "Компьютерная" };

            context.Add(type1);
            context.Add(type2);
            context.Add(type3);
            context.Add(type4);
            context.Add(type5);
            context.Add(type6);

            await context.SaveChangesAsync();


            List<string> names = File.ReadAllLines($@"{Directory.GetCurrentDirectory()}\wwwroot\names.txt").ToList();

            // Create Experts
            var expertRepostiry = serviceProvider.GetService<ExpertRepository>();
            var expertManagments = context.ExpertManagments.ToList();
            var typesExpertises = context.TypesExpertise.ToList();
            for (int i = 0; i < CountExpert; i++)
            {
                typesExpertises = typesExpertises.Shuffle();
                var expert = new Expert()
                {
                    ExpertManagment = expertManagments[rnd.Next(expertManagments.Count)],
                    TypesExpertise = typesExpertises.Take(rnd.Next(1, typesExpertises.Count)).ToList(),
                    Rating = rnd.Next(0, 5),
                    Name = names[rnd.Next(names.Count)],
                };

                await expertRepostiry.AddExpertAsync(expert, $"expert{i + 1}@mvd.ru", "123456");
            }


            // Create Employees
            var employeeRepostiry = serviceProvider.GetService<EmployeeRepository>();
            var employeeManagments = context.EmployeeManagments.ToList();
            for (int i = 0; i < CountEmployee; i++)
            {
                var employee = new Employee()
                {
                    EmployeeManagment = employeeManagments[rnd.Next(employeeManagments.Count)],
                    Name = names[rnd.Next(names.Count)]
                };
                await employeeRepostiry.CreateAsync(employee, $"employee{i + 1}@mvd.ru", "123456");
            }

            // Create expertises
            var employees = context.Employees.ToList();
            var experts = context.Experts.Include(x => x.TypesExpertise).ToList();

            for (int i = 0; i < CountExpertise; i++)
            {
                try
                {
                    var createDateTime = DateTime.Now.AddDays(rnd.Next(-20, 0));

                    var expertise = new Expertise()
                    {
                        CreateDateTime = createDateTime,
                        //Questions = "Пример вопросов к эсперту",
                        TermDateTime = createDateTime.AddDays(rnd.Next(40, 60)),
                        Files = new() { }
                    };

                    if (rnd.Next(10) % 3 == 0)
                    {
                        // Accept = true
                        expertise.Accept = true;
                        expertise.AccpetDateTime = createDateTime.AddDays(rnd.Next(0, 60));
                       // expertise.Answers = "Пример ответов эксперта xd";

                        if (rnd.Next(10) % 3 == 0)
                        {
                            expertise.AcceptRating = true;
                            expertise.Rating = rnd.Next(0, 5);
                        }
                    }


                    expertise.Employee = employees[rnd.Next(employees.Count)];
                    expertise.Expert = experts[rnd.Next(experts.Count)];
                    expertise.TypeExpertise = expertise.Expert.TypesExpertise[rnd.Next(expertise.Expert.TypesExpertise.Count)];

                    context.Expertises.Add(expertise);
                }
                catch (Exception exc)
                {

                    throw exc;
                }


            }

            context.SaveChanges();
        }

        
        public async static Task RoleInitialize(IServiceScope scope)
        {

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MyUser>>();
            string adminEmail = "potter27.05@mail.ru";
            string password = "123456Bb!";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("employee"));
            }

            if (await roleManager.FindByNameAsync("expert") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("expert"));
            }

            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("employee"));
            }

            await userManager.CreateUserWithoutEmailConfirm(roleManager, "admin@mvd.ru", "123456", "admin", null);
        }


        public static async Task<MyUser> CreateUserWithoutEmailConfirm(this UserManager<MyUser> userManager,
             RoleManager<IdentityRole> roleManager,
           string email,
           string password,
           string role,
           MyUser user
           )
        {
            if (await userManager.FindByNameAsync(email) == null)
            {
                if (user == null)
                    user = new MyUser();

                user.UserName = email;
                user.Email = email;

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmResult = await userManager.ConfirmEmailAsync(user, code);
                    return user;
                }

            }
            return null;
        }
    }
}