using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace debug_refactor_pack.refactored_code.Models
{
    public class Order
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Customer Customer { get; set; }

        public Order(string productName, decimal unitPrice, int quantity, Customer customer)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be empty.");

            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Price must be greater than zero.");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public decimal Subtotal => UnitPrice * Quantity;
    }
}
