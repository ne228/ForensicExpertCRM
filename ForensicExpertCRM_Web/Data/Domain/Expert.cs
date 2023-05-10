using ForensicExpertCRM_Web.Data;
using ForensicExpertCRM_Web.Data.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForensicExpertCRM_Web.Data.Domain;

public class Expert : MyUser
{

   

    public string Name { get; set; }

    public int Rating { get; set; }



    public List<TypeExpertise> TypesExpertise { get; set; }


    public int ExpertManagmentId { get; set; }
    [ForeignKey("ExpertManagmentId")]
    public ExpertManagment ExpertManagment { get; set; }

}