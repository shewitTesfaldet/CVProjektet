using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_Education
    {
        
            public int CID { get; set; }

            public int EID { get; set; }

            [ForeignKey(nameof(CID))]
            public virtual CV_? CV { get; set; }

            [ForeignKey(nameof(EID))]
            public virtual Education? Education { get; set; }
      

}
}
