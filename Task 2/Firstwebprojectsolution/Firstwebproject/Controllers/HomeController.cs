using Firstwebproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace Firstwebproject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration;

		public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View();
		}

		

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Dashboard()
		{
			return View();
		}

		public IActionResult EditPassword(int accountId)
		{
			using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("Default")))
			{
				connection.Open();

				using (MySqlCommand command = new MySqlCommand("get_account_by_id", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@sp_accountid", accountId);

					using (MySqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							Account account = new Account
							{
								AccountId = reader.GetInt32("accountid"),
								OldPassword = reader.GetString("password")
							};

							return View(account);
						}
					}
				}
			}

			return RedirectToAction("Dashboard");
		}

		[HttpPost]
		public async Task<IActionResult> EditPasswordSubmit(Account account)
		{
			string connectionString = _configuration.GetConnectionString("Default");

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				connection.Open();

				using (MySqlCommand command = new MySqlCommand("update_account", connection))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@sp_accountid", account.AccountId);
					command.Parameters.AddWithValue("@sp_newpass", account.Password);

					command.ExecuteNonQuery();
				}

				connection.Close();
			}

			return RedirectToAction("Dashboard");
		}

		public IActionResult MySchedule()
		{
			return View();
		}

		public IActionResult MyPayments()
		{
			return View();
		}

		public IActionResult MyApplications()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult StudentPlan()
		{
			return View();
		}

		public IActionResult USALEmail()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		public IActionResult AdminPage()
		{
			return View();
		}

		public IActionResult EditPasswordSubmit()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}