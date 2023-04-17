using ForensicExpertCRM_Web.Data.Domain;

namespace ForensicExpertCRM_Web.Models.Expertise
{
    public class ExpertiseViewModel
    {
        public string Questions { get; set; }

        public DateTime TermDateTime { get; set; }

        //public List<MyFile> Files { get; set; }

        public int TypeExpertiseId { get; set; }
        public List<IFormFile> uploads { get; set; }

        public string ExpertId { get; set; }

    }
}
