using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Net.Http;


namespace ForensicExpertCRM_Web.Services
{
    public class Logic
    {

        private readonly ApplicationDbContext _context;
        private readonly Geocoder _geocoder;
        public Logic(Geocoder geocoder, ApplicationDbContext context)
        {
            _context = context;
            _geocoder = geocoder;
        }

        public async Task<List<ExpertOut>> GetExperts(int typeExpertiseId, string employeeId)
        {

            var employee = await _context.Employees.Include(x => x.EmployeeManagment)
                .FirstOrDefaultAsync(x => x.Id == employeeId);
            if (employee == null) throw new Exception("Неверный id польователя");

            var experts = await _context.Experts
                .Include(x => x.ExpertManagment)
                .Where(x => x.TypesExpertise.Any(x => x.Id == typeExpertiseId))
                .ToListAsync();

            var expertInput = new List<ExpertInput>();
            var expertOut = new List<ExpertOut>();
            foreach (var expert in experts)
            {
                var input = new ExpertInput(expert.Id, employeeId, expert.ExpertManagment.Id, _context, _geocoder);
                await input.Init();
                expertInput.Add(input);
                expertOut.Add(new ExpertOut(expertInput.LastOrDefault()));
            }
            expertOut = expertOut.OrderByDescending(x => x.Rating).ToList();


            await Console.Out.WriteLineAsync(JsonConvert.SerializeObject(expertInput,Formatting.Indented));
            return expertOut;
        }



    }
}
