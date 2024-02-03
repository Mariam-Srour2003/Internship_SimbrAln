using Firstwebproject.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace Firstwebproject.Controllers
{
	public class StudentController : Controller
	{

		private readonly ILogger<StudentController> _logger;
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public StudentController(ILogger<StudentController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}


		public IActionResult Students()
		{
			string connectionString = _configuration.GetConnectionString("Default");
			List<Student> students = new List<Student>();

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				using (MySqlCommand command = new MySqlCommand("get_students", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
					{
						DataTable db = new DataTable();
						dataAdapter.Fill(db);

						foreach (DataRow row in db.Rows)
						{
							int id = Convert.ToInt32(row["student_id"]);
							string name = row["name"].ToString();
							string email = row["email"].ToString();
							string phone = row["Lebanon_phone_nb"].ToString();
							int? account = row["hisaccount"] != DBNull.Value ? Convert.ToInt32(row["hisaccount"]) : (int?)null;

							students.Add(new Student { Id = id, Name = name, Email = email, Phone = phone, Account = account });
						}
					}
				}
			}

			return View(students);
		}

		[HttpGet]
		public IActionResult AddStudent()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddStudent(Student student)
		{
			string connectionString = _configuration.GetConnectionString("Default");


			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				using (MySqlCommand command = new MySqlCommand("add_student", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@name", student.Name);
					command.Parameters.AddWithValue("@email", student.Email);
					command.Parameters.AddWithValue("@Lebanon_phone_nb", student.Phone);


					command.ExecuteNonQuery();
				}

				connection.Close();
			}

			return RedirectToAction("Students");
		}

		public IActionResult AddTestScore()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddTestScore([FromForm] Student student)
		{
			string connectionString = _configuration.GetConnectionString("Default");
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				using (MySqlCommand command = new MySqlCommand("add_student_test_scores", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@studentid", student.Id);
					command.Parameters.AddWithValue("@score", student.TestScore);
					command.Parameters.AddWithValue("@testid", student.TestId);

					command.ExecuteNonQuery();
				}
				connection.Close();
			}
			return RedirectToAction("AddTestScore");
		}

	}
}
