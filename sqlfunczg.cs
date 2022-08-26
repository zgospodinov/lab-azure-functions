using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Collections.Generic;
using azure_functions;

namespace Company.Function
{
    public static class sqlfunczg
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> RunProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            using SqlConnection conn = Shared.GetConnection();

            List<Product> products = new List<Product>();
            string query = "SELECT ProductID, ProductName, Quantity FROM Products";

            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2)
                    };

                    products.Add(product);
                }
            }

            return new OkObjectResult(products);
        }

        [FunctionName("GetProduct")]
        public static async Task<IActionResult> RunProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (int.TryParse(req.Query["id"], out var productId))
            {
                using SqlConnection conn = Shared.GetConnection();

                List<Product> products = new List<Product>();
                string query = $"SELECT ProductID, ProductName, Quantity FROM Products WHERE ProductID={productId}";

                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);

                var product = new Product();

                try
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        product.ProductID = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                        product.Quantity = reader.GetInt32(2);
                    }

                    return new OkObjectResult(product);
                }
                catch (System.Exception ex)
                {

                    var response = "No records found";
                    return new OkObjectResult(response);
                }
            }
            else
            {
                return new OkObjectResult("Query parameter id is missing");
            }
        }
    }
}
