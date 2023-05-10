using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ForensicExpertCRM_Web.Services
{
    public class ExpertInput
    {
        private readonly ApplicationDbContext context;

        private ExpertManagment ExpertManagment { get; set; }

        private Expert? Expert { get; set; }

        private Employee? Employee { get; set; }

        private TypeExpertise? TypeExpertise { get; set; }

        private Geocoder _geocoder { get; set; }

        public string Id { get; set; }

        public double Def { get; set; }

        public double Speed { get; set; }

        public double PercentTerm { get; set; }

        public double Destination { get; set; }

        public double Raiting { get; set; }
        public string Name { get; private set; }

        public List<float> ToFloat()
        {

            var list = new List<float>
            {
                (float)Def,
                (float)Speed,
                (float)PercentTerm,
                (float)Destination,
                (float)Raiting
            };

            return list;

        }

        public ExpertInput(string expertId, string employeeId, int typeExpertiseId, ApplicationDbContext context, Geocoder geocoder)
        {
            this.context = context;
            Expert = context.Experts
              .Include(x => x.ExpertManagment)
              .FirstOrDefault(x => x.Id == expertId);

            if (Expert != null)
                ExpertManagment = Expert.ExpertManagment;

            TypeExpertise = context.TypesExpertise.FirstOrDefault(x => x.Id == typeExpertiseId);

            Employee = context.Employees
                .Include(x => x.EmployeeManagment)
                .FirstOrDefault(x => x.Id == employeeId);

            _geocoder = geocoder;
            Name = Expert.Name;
            Id = expertId;

        }
        public double Sigmoid(double value)
        {

            double key = Math.Tanh(value);
            return key;
            double k = Math.Exp(value);
            return k / (1.0f + k);
        }
        public async Task Init()
        {
            Def = Sigmoid(await GetDef());
            Speed = Sigmoid(await GetSpeed());
            PercentTerm = Sigmoid(await GetPercentTerm());
            Destination = 0;//Sigmoid(await GetDestination());
            Raiting = Sigmoid(await GetRating());
        }

        private async Task<double> GetDef()
        {

            //Количество экспертиз, находящихся в производстве
            //var countExpert = context.Expertises.Where(x =>
            //!x.Accept && 
            //x.Expert.ExpertManagment.Id == ExpertManagment.Id &&
            //x.TypeExpertise.Id == TypeExpertise.Id);

            // Количество сотрудников, которые занимается конкретным видом экспертизы

            var countExpertise = await context.Expertises.CountAsync(x =>
                     !x.Accept && x.Expert.Id == Expert.Id);

            var res = countExpertise == 0 ? 0 : 1.0 / countExpertise;
            return res;
        }

        private async Task<double> GetSpeed()
        {
            //speed = ((maxTerm – currentTerm) / maxTerm )
            double avrSpeed = 0;


            // Количество законченных экспертиз
            var acceptedExpertises = await context.Expertises.Where(x =>
                     x.Accept && x.Expert.Id == Expert.Id)
                     .ToListAsync();

            foreach (var acceptedExpertise in acceptedExpertises)
            {
                TimeSpan maxTerm = (TimeSpan)(acceptedExpertise.TermDateTime - acceptedExpertise.CreateDateTime);
                TimeSpan currentTerm = (TimeSpan)(acceptedExpertise.TermDateTime - acceptedExpertise.AccpetDateTime);

                var up = (maxTerm.TotalMinutes - currentTerm.TotalMinutes);
                var down = maxTerm.TotalMinutes;

                var speed = 1 - (up / down);
                avrSpeed += speed;
            }

            if (acceptedExpertises.Count == 0)
                return 0;

            avrSpeed /= acceptedExpertises.Count();

            return avrSpeed;
        }

        private async Task<double> GetPercentTerm()
        {
            var acceptedExpertises = await context.Expertises.Where(x =>
                 x.Accept && x.Expert.Id == Expert.Id)
                 .ToListAsync();

            int countTerm = 0;

            foreach (var acceptedExpertise in acceptedExpertises)
            {
                TimeSpan Term = (TimeSpan)(acceptedExpertise.TermDateTime - acceptedExpertise.AccpetDateTime);
                if (Term.TotalSeconds > 0)
                    countTerm++;
            }

            if (acceptedExpertises.Count == 0)
                return 0;

            double res = (double)((double)countTerm / (double)acceptedExpertises.Count);

            return res;
        }
        private async Task<double> GetDestination()
        {
            var firtAddress = Employee.EmployeeManagment.Address;
            var destAddress = Expert.ExpertManagment.Address;
            var coordFirts = _geocoder.GetCoordinatesAsync(firtAddress).Result;
            var coordDest = _geocoder.GetCoordinatesAsync(destAddress).Result;
            var destination = _geocoder.Distance(coordDest.lat, coordDest.lng, coordFirts.lat, coordFirts.lng);
            return destination;
        }

        private async Task<double> GetRating()
        {
            var expertise = await context.Expertises
                .Where(x => x.Expert.Id == Expert.Id && x.AcceptRating)
                .ToListAsync();
            if (expertise.Count == 0) return 0;

            double rating = 0;
            foreach (var item in expertise)
                rating += item.Rating;

            rating = rating / expertise.Count;
            return rating;
        }
    }
}
