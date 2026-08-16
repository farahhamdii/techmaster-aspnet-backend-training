using System;
using debug_refactor_pack.refactored_code.Enums;
using debug_refactor_pack.refactored_code.Models;
using debug_refactor_pack.refactored_code.Services;

namespace debug_refactor_pack.refactored_code
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
       
                Console.Write("Enter customer name: ");
                string customerName = Console.ReadLine();

                Console.Write("Enter product name: ");
                string productName = Console.ReadLine();

                Console.Write("Enter product price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.Write("Enter quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                Console.Write("Enter customer type (Regular/Silver/Gold/VIP): ");
                string typeInput = Console.ReadLine();

                CustomerType customerType;
                if (!Enum.TryParse(typeInput, true, out customerType))
                {
                    customerType = CustomerType.Regular;
                }

                Customer customer = new Customer(customerName, customerType);
                Order order = new Order(productName, price, quantity, customer);
                OrderCalculator calculator = new OrderCalculator();

         
                decimal subtotal = order.Subtotal;
                decimal discount = calculator.CalculateDiscount(order);
                decimal amountAfterDiscount = subtotal - discount;
                decimal tax = calculator.CalculateTax(amountAfterDiscount);
                decimal shipping = calculator.CalculateShipping(amountAfterDiscount);
                decimal finalTotal = amountAfterDiscount + tax + shipping;

                Console.WriteLine("\n--- Invoice Summary ---");
                Console.WriteLine("Customer: " + customer.Name + " (" + customer.Type + ")");
                Console.WriteLine("Product: " + order.ProductName);
                Console.WriteLine("Price: " + order.UnitPrice);
                Console.WriteLine("Quantity: " + order.Quantity);
                Console.WriteLine("Subtotal: " + subtotal);
                Console.WriteLine("Discount: " + discount);
                Console.WriteLine("Tax (14%): " + tax);
                Console.WriteLine("Shipping: " + shipping);
                Console.WriteLine("Final Total: " + finalTotal);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}