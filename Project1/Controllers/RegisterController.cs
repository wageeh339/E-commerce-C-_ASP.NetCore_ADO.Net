using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace YourNamespace.Controllers
{
    public class RegisterController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-R3CQ3QE;Initial Catalog=DBtest;Integrated Security=True;Encrypt=False";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username, string password, string confirmPassword , string Country , string Email , int Age)
        {
            // Validate password and confirmPassword match
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View("Index");
            }

            // Check if the username already exists
            if (IsUsernameTaken(username))
            {
                ViewBag.Error = "Username is already taken";
                return View("Index");
            }

            // Hash the password (you should use a secure hashing algorithm)
            // For simplicity, let's assume there's a method called HashPassword
           

            // Insert the new user into the database
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO WebUsers (Username, Password , Country , Age , Email) VALUES (@Username, @Password ,@Country , @Age , @Email)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Country", Country);
                    command.Parameters.AddWithValue("@Age", Age);
                    command.Parameters.AddWithValue("@Email", Email);
                    
                    command.ExecuteNonQuery();
                }
            }

            // Registration successful, redirect to login page
            return RedirectToAction("Index", "Login");
        }

        private bool IsUsernameTaken(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM WebUsers WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private string HashPassword(string password)
        {
            // Implement a secure hashing algorithm (e.g., bcrypt, Argon2)
            // For simplicity, let's assume there's a method called HashPassword
            return HashPassword(password);
        }
    }
}

