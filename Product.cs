using System.Runtime.ConstrainedExecution;

namespace azure_functions
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}