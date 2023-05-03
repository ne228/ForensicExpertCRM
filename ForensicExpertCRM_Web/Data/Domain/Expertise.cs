namespace ForensicExpertCRM_Web.Data.Domain
{
    public class Expertise
    {

        public int Id { get; set; }
        public DateTime CreateDateTime { get; set; }

        //public string Questions { get; set; }

        //public string? Answers { get; set; }
        
        public bool Accept { get; set; }

        public double Rating { get; set; }

        public bool AcceptRating { get; set; }

        public DateTime? AccpetDateTime { get; set; }    

        public DateTime TermDateTime { get; set; }

        public List<MyFile> Files { get; set; }
        public List<MyFile> ExpertFiles { get; set; }

        public TypeExpertise TypeExpertise { get; set; }

        public Expert Expert { get; set; }

        public Employee Employee { get; set; }


    }
}