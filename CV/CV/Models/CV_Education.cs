using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class CV_Education
    {
        
            public int CID { get; set; }

            public int EdID { get; set; }

            [ForeignKey(nameof(CID))]
            public virtual CV_? CV { get; set; }

            [ForeignKey(nameof(EdID))]
            public virtual Education? Education { get; set; }
      

}
}
