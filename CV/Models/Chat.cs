using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? SenderID { get; set; }

        public virtual User? Sender { get; set; }

        public int ReceiverID { get; set; }

        public virtual User? Receiver { get; set; }

    }
}
