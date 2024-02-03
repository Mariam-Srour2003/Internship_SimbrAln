namespace Firstwebproject.Models
{
	public class Student
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Phone { get; set; } = null!;

		public int? Account { get; set; } = null!;

		public int TestId { get; set; }
		public int TestScore { get; set; }

	}
}
