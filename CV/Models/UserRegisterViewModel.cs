using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
	public class UserRegisterViewModel
	{

		[StringLength(100)]
		[Required(ErrorMessage = "Vänligen skriv ett användarnamn!")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Vänligen skriv lösenord!")]
		[StringLength(100, ErrorMessage = "Lösenordet måste vara minst {2} tecken långt.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
		ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, ett specialtecken och en siffra.")]
		[Compare("ConfirmPassword")]
		public string Password { get; set; }


		[Required(ErrorMessage = "Vänligen bekräfta lösenord!")]
		[DataType(DataType.Password)]
		[Display(Name = "Bekräfta lösenordet")]
		public string ConfirmPassword { get; set; }

		[StringLength(100)]
		[Required(ErrorMessage = "Vänligen skriv en Epost!")]
		public string Epost { get; set; }

		public bool Privat { get; set; }


		[StringLength(100)]
		[Required(ErrorMessage = "Vänligen skriv in ditt Namn!")]
		public string FirstName { get; set; }

		[StringLength(100)]
		[Required(ErrorMessage = "Vänligen skriv in ditt Efternamn!")]
		public string LastName { get; set; }

	}
}