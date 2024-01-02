using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_Competence
    {
        public int CID { get; set; }

        public int CompID { get; set; }


        [ForeignKey(nameof(CID))]
      
        public virtual CV_? CV { get; set; }

        [ForeignKey(nameof(CompID))]
 
        public virtual Competence? Competence { get; set; }
    }
}
