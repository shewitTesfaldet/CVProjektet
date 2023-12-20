using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class Competence
    {
        [Key]
        public int CompID { get; set; }   
        public string Description { get; set; }
        public virtual IEnumerable<CV_Competence> CV_Competences { get; set; } = new List<CV_Competence>();


    }
}
