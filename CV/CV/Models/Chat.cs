using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace CV.Models
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MID { get; set; }
        [StringLength(250)]
        public string? Text { get; set; }
        public DateTime? Date { get; set; }
        public bool? Read { get; set; }

        public int AID { get; set; }

        [ForeignKey(nameof(AID))]
        public virtual User? user { get; set; }

    }
}
