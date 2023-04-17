using System.ComponentModel.DataAnnotations.Schema;

namespace ForensicExpertCRM_Web.Data.Domain
{
    public class Employee : MyUser
    {

        //public int Id { get; set; }

        public string Name { get; set; }


        public int EmployeeManagmentId { get; set; }
        [ForeignKey("EmployeeManagmentId")]
        public EmployeeManagment EmployeeManagment { get; set; }
    }
}