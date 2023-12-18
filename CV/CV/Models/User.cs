using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AID { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public string Epost { get; set; }
        public string Adress { get; set; }
        public bool privat { get; set; }

        public int CID { get; set; }    

        [ForeignKey(nameof(CID))]
        public virtual CV cv { get; set; }

    }
}
