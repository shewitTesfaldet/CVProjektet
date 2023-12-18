namespace CV.Models
{
   public class Project
    {
        public int PID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
