using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class LoginViewModel
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn!")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Vänligen skriv lösenord!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
