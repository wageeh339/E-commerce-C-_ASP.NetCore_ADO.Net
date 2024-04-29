using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Project1.Models;
using System.Collections.Generic;
using System.Data;

namespace Project1.Controllers
{
    public class CartController : Controller
    {
     List<CartItems> Items = new List<CartItems>();

        private readonly string _connectionString = "Data Source=DESKTOP-R3CQ3QE;Initial Catalog=DBtest;Integrated Security=True;Encrypt=False";

        public IActionResult Index()
        {
            ViewBag.UserID = HttpContext.Session.GetInt32("ID");
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = $"SELECT * FROM Cart WHERE UserID = @UserID ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", ViewBag.UserID);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new CartItems();
                            {
                                item.ProductID = reader.GetInt32(1);
                                item.quantatiy = reader.GetInt32(2);
                                item.Price = reader.GetDouble(3);
                            };
                            Items.Add(item);
                        }
                    }
                }
            }

            return View(Items);

        }
    }
}
