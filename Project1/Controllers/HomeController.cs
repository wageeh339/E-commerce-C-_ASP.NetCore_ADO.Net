using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Project1.Controllers
{

    public class HomeController : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-R3CQ3QE;Initial Catalog=DBtest;Integrated Security=True;Encrypt=False";
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserID = HttpContext.Session.GetInt32("ID");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://fakestoreapi.com/products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsAsync<List<ProductViewModel>>();
                ViewBag.Username = HttpContext.Session.GetString("Name");
                string countQuery = "SELECT COUNT(*) FROM Cart WHERE UserID = @UserID";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(countQuery, connection))
                    {
                        // Add parameter for the user ID
                        command.Parameters.AddWithValue("@UserID", ViewBag.UserID);

                        // Open the connection
                        connection.Open();

                        // Execute the command and get the count
                        int count = (int)command.ExecuteScalar();

                        // Store the count in ViewBag
                        HttpContext.Session.SetInt32("ItemCount", count);
                        ViewBag.UserColumnCount = HttpContext.Session.GetInt32("ItemCount");

                        // Close the connection
                        connection.Close();
                    }
                }




                return View(products);

            }
            else
            {
                _logger.LogError("Failed to fetch product data from the API");
                return View();
            }


        }
        [HttpPost]
        public IActionResult AddToCart(int productId, decimal productPrice , string producttitle)
        {
            // Use the productId and productPrice here to add the product to the cart
            // For demonstration purposes, you can just store them in session
            var ID = productId;
            var Price =  productPrice;
            
            ViewBag.UserID = HttpContext.Session.GetInt32("ID");

            string insertQuery = "INSERT INTO Cart (UserID,ItemID, Quantatiy, total, Done) VALUES (@UserID,@ItemID, @Quantatiy, @total, @Done)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@UserID", ViewBag.UserID);
                    command.Parameters.AddWithValue("@ItemID", productId);
                    command.Parameters.AddWithValue("@Quantatiy", 1);
                    command.Parameters.AddWithValue("@total", productPrice); // You can change the quantity as per your requirement
                    command.Parameters.AddWithValue("@Done", 0); // Assuming Done is a flag indicating whether the product is purchased or not

                    // Open the connection
                    connection.Open();

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Close the connection
                    connection.Close();
                }
            }

            string countQuery = "SELECT COUNT(*) FROM Cart WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(countQuery, connection))
                {
                    // Add parameter for the user ID
                    command.Parameters.AddWithValue("@UserID", ViewBag.UserID);

                    // Open the connection
                    connection.Open();

                    // Execute the command and get the count
                    int count = (int)command.ExecuteScalar();

                    // Store the count in ViewBag
                    ViewBag.UserColumnCount = count;

                    // Close the connection
                    connection.Close();
                }
            }
            // Redirect to the Index action after adding to cart
            return RedirectToAction("Index");
        }
    }
}
