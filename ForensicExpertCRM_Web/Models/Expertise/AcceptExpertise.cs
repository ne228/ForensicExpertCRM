using System.ComponentModel.DataAnnotations;

namespace ForensicExpertCRM_Web.Models.Expertise
{
    public class AcceptExpertise
    {
        public int ExpertiseId { get; set; }

        //[Required]
        //public string Answers { get; set; }

        public List<IFormFile>? uploads { get; set; }
    }
}
