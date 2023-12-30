using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class UserRegisterViewModel
    {
        
        [StringLength(100)]
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vänligen skriv lösenord!")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vänligen bekräfta lösenord!")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenordet")]
        public string ConfirmPassword { get; set; }


    }
}
