using ForensicExpertCRM_Web.Data.Domain;
using ForensicExpertCRM_Web.Models.Expert;

namespace ForensicExpertCRM_Web.Models.Employee
{
    public class EmployeeViewModel
    {
        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int EmployeeManagmentId { get; set; }

        //public List<EmployeeManagmentRadioBtnModel> EmployeeManagments { get; set; }


        public Data.Domain.Employee ToEntity()
        {
            var res = new Data.Domain.Employee()
            {
                Name = Name,
                Email = Login,
                PasswordHash = Password,
                EmployeeManagmentId = EmployeeManagmentId
                
                
            };
     
            return res;
        }
    }

    public class EmployeeManagmentRadioBtnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }


    public static class EmployeeModelExtension
    {
        public static List<EmployeeManagmentRadioBtnModel> EmployeeManagmentToCheckBoxModel(this List<EmployeeManagment> typeExpertises)
        {
            var model = new List<EmployeeManagmentRadioBtnModel>();
            foreach (var type in typeExpertises)
            {
                model.Add(new EmployeeManagmentRadioBtnModel() { Id = type.Id, Name = type.Name, IsSelected = false });
            }
            return model;
        }

        public static List<EmployeeManagment> ToEntity(this List<EmployeeManagmentRadioBtnModel> expertManagmentRadioBtnModels)
        {
            var res = new List<EmployeeManagment>();
            foreach (var item in expertManagmentRadioBtnModels)
            {
                if (item.IsSelected == true)
                    res.Add(new EmployeeManagment()
                    {
                        Id = item.Id,
                        Name = item.Name

                    });
            }

            return res;
        }

    }
}