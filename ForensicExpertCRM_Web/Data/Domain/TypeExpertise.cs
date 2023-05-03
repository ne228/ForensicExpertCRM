using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForensicExpertCRM_Web.Data.Domain
{
    public class TypeExpertise
    {
        public int Id { get; set; }


        [Display(Name = "Название")]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Expert> Experts { get; set; }
    }
}
