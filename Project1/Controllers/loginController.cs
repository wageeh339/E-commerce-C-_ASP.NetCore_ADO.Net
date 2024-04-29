using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;


public class LoginController : Controller
{
    private readonly string _connectionString = "Data Source=DESKTOP-R3CQ3QE;Initial Catalog=DBtest;Integrated Security=True;Encrypt=False";

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Authenticate(string username, string password)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = $"SELECT ID, UserName FROM WebUsers WHERE UserName = '{username}' AND Password = '{password}'";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(0); // Get the value of the first column (UserId)
                        string usernameString = reader.GetString(1); // Get the value of the second column (UserName)

                        HttpContext.Session.SetInt32("ID", userId); // Store the integer value in session
                        HttpContext.Session.SetString("Name", usernameString);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }
        // Authentication failed
        ViewBag.Error = "Invalid username or password";
        return View("Index");
    }
}
