using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_projekt
    {
        [Key]
        public int CID { get; set; }

        [Key]
        public int PID { get; set; }


        [ForeignKey(nameof(CID))]
        public virtual CV? cv { get; set; }



        [ForeignKey(nameof(PID))]
        public virtual Project? Project { get; set; }


    }
}
