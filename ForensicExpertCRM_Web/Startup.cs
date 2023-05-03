using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Test_OP_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();





            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<MyUser, IdentityRole>(opts =>
                   {
                       opts.Password.RequiredLength = 5;   // минимальная длина
                       opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                       opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                       opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                       opts.Password.RequireDigit = false; // требуются ли цифры
                   })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddScoped<ExpertRepository>();
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<Geocoder>(_ => new Geocoder(_.GetRequiredService<HttpClient>(), "f24181bd-5a42-48ff-80aa-ce61e9f67c49"));
            services.AddScoped<Logic>();
            services.AddHttpClient();

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Expertises}/{action=Index}/{id?}");
            });
        }
    }
}
