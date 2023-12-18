using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class CV_
    {
        [Key]
        public int CID { get; set; }

        //koppla 1 - många
        public virtual IEnumerable<Experience> Experience { get; set; }

        //koppla 1 - många eller många till många med tydliga dokument. Ska kopplas till CV. Jonas tycker många till många. 

        public virtual IEnumerable <Education> Education { get; set; }

        //koppla 1 - många eller många till många med tydliga dokument. Ska kopplas till CV. Jonas tycker många till många.
        public virtual IEnumerable<Competence> Competence { get; set; }

        public string Picture { get; set; }


    }
}
