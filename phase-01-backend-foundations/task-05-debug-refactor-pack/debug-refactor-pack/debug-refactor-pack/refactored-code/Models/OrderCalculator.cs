using debug_refactor_pack.refactored_code.Enums;
using debug_refactor_pack.refactored_code.Models;
using System;

namespace debug_refactor_pack.refactored_code.Services
{
    public class OrderCalculator
    {
       
        private const decimal TaxRate = 0.14m;
        private const decimal ShippingFee = 50.0m;
        private const decimal FreeShippingLimit = 1000.0m;

        public decimal CalculateDiscount(Order order)
        {
            decimal subtotal = order.Subtotal;

            switch (order.Customer.Type)
            {
                case CustomerType.Silver:
                    return subtotal * 0.05m;
                case CustomerType.Gold:
                    return subtotal * 0.10m;
                case CustomerType.VIP:
                    return subtotal * 0.15m;
                default:
                    return 0.0m;
            }
        }
        public decimal CalculateTax(decimal amountAfterDiscount)
        {
            return amountAfterDiscount * TaxRate;
        }
        public decimal CalculateShipping(decimal amountAfterDiscount)
        {
            if (amountAfterDiscount >= FreeShippingLimit)
            {
                return 0.0m;
            }

            return ShippingFee;
        }
    }
}