using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_Competence
    {
        public int CID { get; set; }

        public int COID { get; set; }

        [ForeignKey(nameof(CID))]
        public virtual CV_? CV { get; set; }

        [ForeignKey(nameof(COID))]
        public virtual Competence? Competence { get; set; }
    }
}
