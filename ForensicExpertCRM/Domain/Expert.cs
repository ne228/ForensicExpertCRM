namespace ForensicExpertCRM.Domain
{
    public class Expert
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Rating { get; set; }

        public List<TypeExpertise> TypesExpertise { get; set; }

    }
}