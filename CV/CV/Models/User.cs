using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UID { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Du måste ange ett användarnamn!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Du måste ange ett förnamn!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Du måste ange ett efternamn!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Du måste ange ett lösenord!")]

        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Vänlingen bekräfta lösenordet")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekrafta losenordet")]
        public string ConfirmPassword { get; set; }
        public string Epost { get; set; }
        public string Adress { get; set; }
        public bool Privat { get; set; }

        public int CID { get; set; }

        [ForeignKey(nameof(CID))]
        public virtual CV_? Cv { get; set; }

    }
}
