namespace ForensicExpertCRM_Web.Data.Domain
{
    public class Expertise
    {

        public int Id { get; set; }
        public DateTime CreateDateTime { get; set; }

        public string Questions { get; set; }

        public string? Answers { get; set; }


        public DateTime TermDateTime { get; set; }

        public List<MyFile> Files { get; set; }

        public TypeExpertise TypeExpertise { get; set; }

        public Expert Expert { get; set; }

        public Employee Employee { get; set; }


    }
}