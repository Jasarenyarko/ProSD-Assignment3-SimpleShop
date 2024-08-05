namespace SimpleShop{
    
    public class Customer{
        public const decimal ValueAddedTax = 0.19m;
        public string Name = "";

        public string Type = "";
        public virtual decimal CalculatePrice(decimal basePrice){
            return (1 + ValueAddedTax) * basePrice;
        }

        public static Customer CreateCustomer(string name, string customerType=""){
        if (customerType.ToLower() == "student") {
                return new Student(name);
            } else if (customerType.ToLower() == "company") {
                return new Company(name);
            } else {
                return new Customer { Name = name, Type = customerType };
            }
        }  

            
    }

     public class Company : Customer
        {
        public Company() {}
        public Company(string name)
        {
        this.Name = name;
        this.Type = "Company"; 
        }

        public override decimal CalculatePrice(decimal basePrice) {
        return basePrice;
        }
        } 

      public class Student : Customer {
            public Student() {}
        public Student(string name) {
            this.Name = name;
            this.Type = "Student";
        }

        public override decimal CalculatePrice(decimal basePrice) {
            // Apply 20% discount before adding VAT
            decimal discountedPrice = basePrice * 0.8m; // 20% discount
            return (1 + ValueAddedTax) * discountedPrice;
        }
    }

    
}