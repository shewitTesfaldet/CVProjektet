using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class CV2
    {
        [Key]
        public int CID { get; set; }

        public string Experience { get; set; }

        public string Education { get; set; }

        public string Competence { get; set; }

        public string Picture { get; set; }


    }
}
