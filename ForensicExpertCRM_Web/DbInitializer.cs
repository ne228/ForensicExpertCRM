using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.AspNetCore.Identity;

namespace Test_OP_Web
{
    public static class DbInitializer
    {

        public async static Task Initialize(ApplicationDbContext context)
        {
            //context.Database.EnsureDeleted();
            if (context.Database.EnsureCreated())
            {
                var type1 = new TypeExpertise() { Name = "Судебная" };
                var type2 = new TypeExpertise() { Name = "Почерк" };
                context.Add(type2);
                context.Add(type1);

                var expertManagment1 = new ExpertManagment() { Name = "ГУ МВД", Address = "Битовск" };
                var expertManagment2 = new ExpertManagment() { Name = "ГУ МВД по Битовску", Address = "Битовск" };
                context.Add(expertManagment1);
                context.Add(expertManagment2);


                var EmployeeManagment1 = new EmployeeManagment() { Name = "ГУ МВД", Address = "Битовск" };
                var EmployeeManagment2 = new EmployeeManagment() { Name = "ГУ МВД по Битовску", Address = "Битовск" };
                context.Add(EmployeeManagment1);
                context.Add(EmployeeManagment2);


                await context.SaveChangesAsync();

            }
        }

        public async static Task RoleInitialize(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {

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
                //var user = new Expert { Email = email, UserName = email };
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