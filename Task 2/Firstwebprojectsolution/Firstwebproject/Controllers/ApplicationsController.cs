using Firstwebproject.Models;
using Firstwebproject;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging; 
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using MySqlX.XDevAPI.Common;
using Google.Protobuf.WellKnownTypes;
using static System.Net.Mime.MediaTypeNames;

namespace Firstwebproject.Controllers
{
    public class ApplicationsController : Controller
    {

        private readonly ILogger<ApplicationsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly string _connectionString;

        public ApplicationsController(EmailService emailService, ILogger<ApplicationsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationNotes(int appid, string selectedNote)
        {
            string connectionString = _configuration.GetConnectionString("Default");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("addnotes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@appid", appid);
                    command.Parameters.AddWithValue("@notes", selectedNote);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return RedirectToAction("ApplicationsAccept");
        }


        [HttpGet]
        public IActionResult NewApplication()
        {
            return View();
        }

        public IActionResult UpdateApplicationStatus()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewApplication(StudentApplication application)
        {
            string connectionString = _configuration.GetConnectionString("Default");


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("addnewapplicationnewstudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@sname", application.Name);
                    command.Parameters.AddWithValue("@semail", application.Email);
                    command.Parameters.AddWithValue("@sphone", application.LebanonPhoneNumber);
                    string selectedGender = Request.Form["radiogroup1"];
                    command.Parameters.AddWithValue("@sgender", selectedGender);
                    command.Parameters.AddWithValue("@sage", application.Age);
                    command.Parameters.AddWithValue("@saddress", application.Address);
                    string selectedProgram = Request.Form["programName"];
                    command.Parameters.AddWithValue("@pprogram", selectedProgram);
                    StringBuilder selectedCheckboxes = new StringBuilder();
                    if (Request.Form["lebaneseBaccalaureate"] == "on")
                    {
                        selectedCheckboxes.Append("Lebanese Baccalaureate Certificate or its equivalent, ");
                    }

                    if (Request.Form["universityCertificate"] == "on")
                    {
                        selectedCheckboxes.Append("University Certificate or its equivalent, ");
                    }
                    if (selectedCheckboxes.Length > 0)
                    {
                        selectedCheckboxes.Length -= 2;
                    }
                    command.Parameters.AddWithValue("@pgraduates", selectedCheckboxes.ToString());
                    string toEmail = application.Email;
                    string subject = "Application Send";
                    string message = "Hello,\r\n\r\nWe wanted to inform you that an action was successfully performed in our application. We thought you'd like to know about it!";

                    _emailService.SendEmailAsync(toEmail, subject, message).Wait();


                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return RedirectToAction("NewApplication");
        }


        public IActionResult ApplicationsAccept()
        {
            string connectionString = _configuration.GetConnectionString("Default");
            List<StudentApplication> StudentApplications = new List<StudentApplication>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("get_applications", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command))
                    {
                        DataTable db = new DataTable();
                        dataAdapter.Fill(db);

                        foreach (DataRow row in db.Rows)
                        {
                            int id = Convert.ToInt32(row["application_id"]);
                            string name = row["name"].ToString();
                            string email = row["email"].ToString();
                            string phone = row["Lebanon_phone_nb"].ToString();
                            string progname = row["program_name"].ToString();
                            string grad = row["graduates"].ToString();
                            bool status = (bool)row["application_status"];
                            string notes = row["applivation_notes"].ToString();

                            StudentApplications.Add(new StudentApplication { ID = id, Name = name, Email = email, LebanonPhoneNumber = phone, ProgramName = progname, Graduates = grad, ApplicationStatus = status, ApplicationNotes = notes });
                        }
                    }
                }
            }
            return View(StudentApplications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(int appid)
        {
            string connectionString = _configuration.GetConnectionString("Default");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using var command = new MySqlCommand("accept_application", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@appid", appid);

                // Execute the stored procedure
                command.ExecuteNonQuery();

                // Now retrieve the email and account ID separately
                string getEmailAndAccountIdQuery = "SELECT s.email, s.hisaccount FROM applications a JOIN students s ON a.applicationforstudent = s.student_id WHERE a.application_id = @appid";

                using var getEmailAndAccountIdCommand = new MySqlCommand(getEmailAndAccountIdQuery, connection);
                getEmailAndAccountIdCommand.Parameters.AddWithValue("@appid", appid);

                using var reader = getEmailAndAccountIdCommand.ExecuteReader();
                if (reader.Read())
                {
                    string toEmail = reader["email"].ToString();
                    int accountId = Convert.ToInt32(reader["hisaccount"]);

                    string subject = "Application Accepted";
                    string message = $"Hello,\r\n\r\nWe wanted to inform you that your application was accepted. We thought you'd like to know about it! Your account id is: {accountId} and pass S@s123456 change it as soon as possible";

                    await _emailService.SendEmailAsync(toEmail, subject, message);
                }

                reader.Close();
            }

            return RedirectToAction("ApplicationsAccept");
        }





    }
}


        

           
            
          


