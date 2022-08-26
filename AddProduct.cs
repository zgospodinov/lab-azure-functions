using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using azure_functions;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Company.Function
{
    public static class AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            Product product = JsonConvert.DeserializeObject<Product>(reqBody);

            using SqlConnection conn = Shared.GetConnection();

            var command = "INSERT INTO Products (ProductID, ProductName, Quantity) VALUES (@param1, @param2, @param3)";

            conn.Open();
            using (SqlCommand cmd = new SqlCommand(command, conn))
            {
                cmd.Parameters.Add("@param1", System.Data.SqlDbType.Int).Value = product.ProductID;
                cmd.Parameters.Add("@param2", System.Data.SqlDbType.NVarChar).Value = product.ProductName;
                cmd.Parameters.Add("@param3", System.Data.SqlDbType.Int).Value = product.Quantity;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            
            return new OkObjectResult("Product added");
        }
    }
}
