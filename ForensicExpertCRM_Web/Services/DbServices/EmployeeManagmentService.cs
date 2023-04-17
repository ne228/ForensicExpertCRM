using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;

namespace ForensicExpertCRM_Web.Services.DbServices
{
    public class EmployeeManagmentService
    {

        private readonly ApplicationDbContext _context;

        public EmployeeManagmentService(ApplicationDbContext context, ILogger<EmployeeManagmentService> logger)
        {
            _context = context;
            Logger = logger;
        }

        public ILogger<EmployeeManagmentService> Logger { get; }

        public async Task Create(string name, string address)
        {
            try
            {
                var managment = new EmployeeManagment
                {
                    Name = name,
                    Address = address,
                    Employees = new List<Employee>()
                };
                await _context.AddAsync(managment);
            }
            catch (Exception exc)
            {

                Logger.LogError(exc.Message);
            }            
        }

        public async Task Edit(string id, string name, string address)
        {

            try
            {
                var managment = new EmployeeManagment { Name = name, Address = address };
                await _context.AddAsync(managment);
            }
            catch (Exception exc)
            {

                Logger.LogError(exc.Message);
            }
        }



    }
}
