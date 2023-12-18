using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class Experience
    {
        [Key]
        public int EID { get; set; }
        public string Description { get; set; }
        public int CID { get; set; }

        [ForeignKey(nameof(CID))]
        public virtual CV_? user { get; set; }

    }
}
