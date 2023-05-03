
using System.ComponentModel.DataAnnotations;

namespace ForensicExpertCRM_Web.Models.Expertise
{
    public class ExpertiseViewModel
    {

        //[Required(ErrorMessage = "Введите вопросы поставленные перед экспертом")]
        //public string Questions { get; set; }


        [Required(ErrorMessage = "Выберите предельный срок окончания экспертизы")]
        [DataType(DataType.DateTime, ErrorMessage = "Выберите предельный срок окончания экспертизы")]
        public DateTime? TermDateTime { get; set; }



        [Required(ErrorMessage = "Выберите тип экспертизы")]
        public int TypeExpertiseId { get; set; }



        public List<IFormFile>? uploads { get; set; }

        [Required(ErrorMessage = "Выберите эксперта")]
        public string ExpertId { get; set; }

    }
}
