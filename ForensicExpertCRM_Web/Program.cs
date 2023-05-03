using ForensicExpertCRM_Web;
using ForensicExpertCRM_Web.Data;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using TelegramSink;

namespace Test_OP_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var app = CreateHostBuilder(args);

            var logger = GetLogger();
            app.UseSerilog(logger);

            var host = app.Build();
            CreateDbIfNotExists(host).Wait();

            host.Run();
        }

        private static async Task CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    var userManager = services.GetRequiredService<UserManager<MyUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DbInitializer.Initialize(scope, context);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static Serilog.Core.Logger GetLogger()
        {
            var logger = new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       // Add console (Sink) as logging target
                       .WriteTo.Console()

                       // Write logs to a file for warning and logs with a higher severity
                       // Logs are written in JSON
                       .WriteTo.File(
                           new JsonFormatter(),
                           $"{Directory.GetCurrentDirectory()}/logs/important-logs.json",
                           restrictedToMinimumLevel: LogEventLevel.Warning)

                       .WriteTo.TeleSink(
                         telegramApiKey: "5982966206:AAGxc5e5eL8VXRleimsx7l5boPJIPIWcrfE",
                         telegramChatId: "467719960",
                         minimumLevel: LogEventLevel.Warning
                         )
                       // Add a log file that will be replaced by a new log file each day
                       .WriteTo.File(
                                   $"{Directory.GetCurrentDirectory()}/logs/all-daily-.logs",
                                  rollingInterval: RollingInterval.Day)

                   .CreateLogger();

            return logger;
        }
    }
}
