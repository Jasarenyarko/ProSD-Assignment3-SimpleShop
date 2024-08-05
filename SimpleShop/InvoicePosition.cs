using System.Text.RegularExpressions; // required for Regex function
/// Hint: you can use Regex functions to check for non-numerical values into numerical tags like ItemNumber, AmountOrdered, etc.

namespace SimpleShop{
    public class InvoicePosition{
        public uint ItemIdentifier = 0;
        public string ItemName = "";
        public uint Orders = 0;
        public decimal SingleUnitPrice = 0.0m;
        public Customer Customer;

        public virtual decimal Price(){
            return this.Customer.CalculatePrice(this.SingleUnitPrice * Orders);
        }


       public static bool IsValidNumber(string input) {
    // Validate for non-numeric characters or invalid numbers
        return uint.TryParse(input, out var _) && !Regex.IsMatch(input, @"[^0-9]");
        }

       public static bool IsValidDecimal(string input)
    {
        // Trim the trailing 'm' if present
        if (input.EndsWith("m", StringComparison.OrdinalIgnoreCase))
        {
            input = input.Substring(0, input.Length - 1);
        }

         bool success = decimal.TryParse(input, out var _);

         return success;
    }

 

        public static InvoicePosition CreateFromPairs(KeywordPair[] pairs){
             var invoicePosition = new InvoicePosition();
        foreach (var pair in pairs) {
        switch (pair.Key.GetString()) {
            case "ItemNumber":
                if (uint.TryParse(pair.Value, out var itemId)) {
                    invoicePosition.ItemIdentifier = itemId;
                }
                break;
            case "ItemName":
                invoicePosition.ItemName = pair.Value;
                break;
            case "AmountOrdered":
                // Check for valid uint and non-numeric characters
                if (IsValidNumber(pair.Value)) {
                    if (uint.TryParse(pair.Value, out var orders)) {
                        invoicePosition.Orders = orders;
                    }
                } else {
                    // // Handle invalid input by setting default value
                    invoicePosition.Orders = 0; // Default value for Orders
                }
                break;
            case "NetPrice":
                // Check for valid decimal and non-numeric characters
                    IsValidDecimal(pair.Value);
                    if (decimal.TryParse(pair.Value, out var price)) {
                        invoicePosition.SingleUnitPrice = price;
                    }
                else {
                    // Handle invalid input by setting default value
                    invoicePosition.SingleUnitPrice = 0.0m; // Default value for SingleUnitPrice
                }
                break;
            case "CustomerName":
                if (invoicePosition.Customer == null) {
                    invoicePosition.Customer = new Customer { Name = pair.Value };
                } else {
                    invoicePosition.Customer.Name = pair.Value;
                }
                break;
            case "CustomerType":
                if (invoicePosition.Customer == null) {
                    invoicePosition.Customer = new Customer();
                }
                invoicePosition.Customer = Customer.CreateCustomer(invoicePosition.Customer.Name, pair.Value);
                break;
        }
        }

        if (invoicePosition.Customer == null) {
        invoicePosition.Customer = new Customer();
        }

    return invoicePosition;
        }
    }
}