using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class CV_
    {
        [Key]
        public int CID { get; set; }

        public virtual IEnumerable<Experience> Experience { get; set; }

        public virtual IEnumerable <Education> Education { get; set; }

        public virtual IEnumerable<Competence> Competence { get; set; }

        public string Picture { get; set; }


    }
}
