using ForensicExpertCRM_Web.Data.Domain;

namespace ForensicExpertCRM_Web.Models.Expert
{
    public class ExpertViewModel
    {
        public string Name { get; set; }


        public int Rating { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public List<TypeExpertiseCheckBoxModel> TypesExpertise { get; set; }

        public List<ExpertManagmentRadioBtnModel> ExpertManagments { get; set; }


        public Data.Domain.Expert ToEntity()
        {
            var res = new Data.Domain.Expert()
            {
                Rating = Rating,
                Name = Name,
                TypesExpertise = TypesExpertise.ToEntity(),
                Email = Login,
                PasswordHash = Password
            };

            return res;
        }

    }

    public class TypeExpertiseCheckBoxModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }


    }

    public class ExpertManagmentRadioBtnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }


    public static class ExpertModelExtension
    {
        public static List<TypeExpertiseCheckBoxModel> TypesExpertiseToCheckBoxModel(this List<TypeExpertise> typeExpertises)
        {
            var model = new List<TypeExpertiseCheckBoxModel>();
            foreach (var type in typeExpertises)
            {
                model.Add(new TypeExpertiseCheckBoxModel() { Id = type.Id, Name = type.Name, IsChecked = false });
            }
            return model;
        }


        public static List<ExpertManagmentRadioBtnModel> ExpertManagmentToCheckBoxModel(this List<ExpertManagment> typeExpertises)
        {
            var model = new List<ExpertManagmentRadioBtnModel>();
            foreach (var type in typeExpertises)
            {
                model.Add(new ExpertManagmentRadioBtnModel() { Id = type.Id, Name = type.Name, IsSelected = false });
            }
            return model;
        }

        public static List<ExpertManagment> ToEntity(this List<ExpertManagmentRadioBtnModel> expertManagmentRadioBtnModels)
        {
            var res = new List<ExpertManagment>();
            foreach (var item in expertManagmentRadioBtnModels)
            {
                if (item.IsSelected == true)
                    res.Add(new ExpertManagment()
                    {
                        Id = item.Id,
                        Name = item.Name

                    });
            }

            return res;
        }
        public static List<TypeExpertise> ToEntity(this List<TypeExpertiseCheckBoxModel> typeExpertiseModel)
        {
            var res = new List<TypeExpertise>();
            foreach (var item in typeExpertiseModel)
            {
                if (item.IsChecked == true)
                    res.Add(new TypeExpertise()
                    {
                        Id = item.Id,
                        Name = item.Name

                    });
            }

            return res;
        }
    }


}
