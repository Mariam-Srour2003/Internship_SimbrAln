using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firstwebproject.Models
{
	public class StudentApplication
	{

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100)]
		public string? Name { get; set; }

		[StringLength(100)]
		public string? Email { get; set; }

		[StringLength(8)]
		public string? LebanonPhoneNumber { get; set; }

		public string? Gender { get; set; }

		public int Age { get; set; }

		[StringLength(100)]
		public string? Address { get; set; }

		[Required(ErrorMessage = "Program name is required.")]
		[StringLength(50)]
		public string? ProgramName { get; set; }

		public int ApplicationForStudent { get; set; }

		public int ProgramApplyingTo { get; set; }

		public bool ApplicationStatus { get; set; } = false;

		[StringLength(200)]
		public string? ApplicationNotes { get; set; }

		[DataType(DataType.Date)]
		public DateTime AppliedDate { get; set; }

		[StringLength(200)]
		public string? Graduates { get; set; }

		[Key]
		public int ID { get; set; }

	}

}