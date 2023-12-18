using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
   public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
