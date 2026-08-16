using debug_refactor_pack.refactored_code.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace debug_refactor_pack.refactored_code.Models
{
    public class Customer
    {
        public string Name { get; set; } = string.Empty;
        public CustomerType Type { get; set; } = CustomerType.Regular;

        public Customer(string name, CustomerType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty.");

            Name = name;
            Type = type;
        }
    }
}
