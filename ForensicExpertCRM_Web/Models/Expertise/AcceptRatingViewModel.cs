using System.ComponentModel.DataAnnotations;

namespace ForensicExpertCRM_Web.Models.Expertise
{
    public class AcceptRatingViewModel
    {
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }

        [Required]
        public int ExpertiseId { get; set; }
    }
}
