using Newtonsoft.Json;

namespace ForensicExpertCRM_Web.Services
{
    public class ExpertOut
    {
        public ExpertOut(ExpertInput expertInput)
        {
            Id = expertInput.Id;
            Name = expertInput.Name;
            Rating = GetRating(expertInput);
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public double Rating { get; set; }


        // TODO NEURO
        private double GetRating(ExpertInput expertInput)
        {
            var json = JsonConvert.SerializeObject(expertInput, Formatting.Indented);

            var result = 0.3 * expertInput.Def + 0.2 * expertInput.Speed + 0.3 * expertInput.PercentTerm + 0.1 * expertInput.Destination + 0.2 * expertInput.Raiting;
            Console.WriteLine(json);
            Console.WriteLine(result);

            //Neuro(expertInput);
            return result;
        }



    }
}


