namespace ForensicExpertCRM_Web.Data.Domain
{
    public class EmployeeManagment
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public List<Employee>? Employees { get; set; }
    }
}
