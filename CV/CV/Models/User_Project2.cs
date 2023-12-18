using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class User_Project2
    {

        [ForeignKey(nameof(AID))]
        public int AID { get; set; }

        [ForeignKey(nameof(PID))]
        public int PID { get; set; }
    }
}
