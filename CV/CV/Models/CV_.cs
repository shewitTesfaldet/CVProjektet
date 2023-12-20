using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CID { get; set; }
        //Ta bort senare detta är för exempeldata
        public virtual IEnumerable <Education> Education { get; set; }
        //Ta bort senare detta är för exempeldata
        public virtual IEnumerable<Competence> Competence { get; set; }

        public string Picture { get; set; }
        public virtual IEnumerable<Experience> Experiences { get; set; } = new List<Experience>();

        public virtual User User { get; set; }

        public virtual IEnumerable<CV_Competence> CV_Competences { get; set; } = new List<CV_Competence>();
        public virtual IEnumerable<CV_Education> CV_Education { get; set; } = new List<CV_Education>();

    }
}
