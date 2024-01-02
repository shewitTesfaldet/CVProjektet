using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class User_Project
    {
        public int UID { get; set; }

        public int PID { get; set; }

        [ForeignKey(nameof(UID))]
        public virtual User? user { get; set; }

        [ForeignKey(nameof(PID))]
        public virtual Project? project { get; set; }
    }
}
