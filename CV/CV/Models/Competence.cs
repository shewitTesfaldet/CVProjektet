using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class Competence
    {
        [Key]
        public int CompID { get; set; }   
        public string Description { get; set; }
        public int CID { get; set; }    


    }
}
