using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class Education
    {
        [Key] 
        public int EdID { get; set; }
        public string Description { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual IEnumerable<CV_Education> CV_Education { get; set; } = new List<CV_Education>();

    }
}
