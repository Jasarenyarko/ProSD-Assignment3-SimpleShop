using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SimpleShop;
// Remember [UnitOfWork_StateUnderTest_ExpectedBehaviour]

namespace SimpleShop.Test
{
    public class Tests
    {
        private ShopParser parser1 = new ShopParser();
        private ShopParser parser2 = new ShopParser();
        private Keyword keyword1;
        private Keyword keyword2;
        private Keyword keyword3;
        private Keyword intKeyword;
        private string tag;
        private Keyword decimalKeyword;
        private Customer customer1 = new Customer();
        private InvoicePosition invoice1;

        
      

        [SetUp]
        public void Setup(){
            keyword1 = new Keyword("Shoe",KeywordTypes.String);
            keyword2 = new Keyword("Shirt");
            keyword3 = new Keyword("Trousers");
            intKeyword= new Keyword("8", KeywordTypes.Int);
            decimalKeyword = new Keyword("0.1",KeywordTypes.Decimal);

        

            invoice1 = new InvoicePosition{
                Customer = Customer.CreateCustomer("Daniel"),
                ItemIdentifier = 0,
                ItemName = "Hat",
                Orders = 20,
                SingleUnitPrice = 10m
            };

            var keywords = new Keyword[] { keyword1, intKeyword, decimalKeyword };

            tag = "<ItemNumber>1</ItemNumber><ItemName>Burger</ItemName><CustomerName>James T. Kirk</CustomerName><AmountOrdered>2</AmountOrdered><NetPrice>8.00</NetPrice>";

            parser1.SetKeywords(keywords);

             parser2.SetKeywords(new Keyword[]
            {
                new Keyword("ItemNumber"),
                new Keyword("ItemName"),
                new Keyword("CustomerName"),
                new Keyword("AmountOrdered"),
                new Keyword("NetPrice")
            });

        }
        
        /// <summary>
        /// Check if the Keyword opening is modified with added <Keyword> 
        /// Rating 1
        /// </summary>
        [Test]
        [Category("Keyword")]
        /// GetStart method of DataTypes.cs is being Tested. To check if it will add <> to keyword as designed
        public void Parsing_KeywordStartTag_AddedBraces()
        {
            string expected = "<Shoe>";

            Assert.That(keyword1.GetStart(), Is.EqualTo(expected));
        }
        
        /// <summary>
        /// Check if the Keyword closing is modified with added </Keyword>
        /// Rating 1
        /// </summary>
        [Test]
        [Category("Keyword")]
        /// GetEnd method of DataTypes.cs is being Tested. To check if it will add </> to keyword as designed
        public void Parsing_KeywordEndTag_AddedSlashAndBraces(){
            string expected = "</Shoe>";

            Assert.That(keyword1.GetEnd(), Is.EqualTo(expected));
        }
        
        /// <summary>
        /// Set the Keywords an check if they are valid.
        /// Rating 1
        /// </summary>
        [Test]
        [Category("ShopParser")]
        /// This test verifies that the keywords parsed into parser1 are in the correct order. 
        /// This tests the SetKeywords method of ShopParser.cs
        public void Parsing_SetKeywords_OrderOfKeywordsIsCorrect(){

             Assert.That(parser1.GetKeywords()[0].GetString(), Is.EqualTo("Shoe"));
             Assert.That(parser1.GetKeywords()[1].GetString(), Is.EqualTo("8"));
             Assert.That(parser1.GetKeywords()[2].GetString(), Is.EqualTo("0.1"));

        }
        
        /// <summary>
        /// Set the Keyword types and check if they are valid.
        /// Rating 1
        /// </summary>
        [Test]
        [Category("ShopParser")]
         /// This test verifies that the keywords parsed into parser1 are in the correct order 
         /// Also each keyword in parser1 returns its accompayning type
        /// This tests the SetKeywords function in ShopParser.cs
        public void ShopParser_SetKeyword_Typ()
        {   
            var expected = new KeywordTypes []{
                KeywordTypes.String,
                KeywordTypes.Int,
                KeywordTypes.Decimal};

            Assert.That(parser1.GetKeywords()[0].WhichType(), Is.EqualTo(expected[0]));
            Assert.That(parser1.GetKeywords()[1].WhichType(), Is.EqualTo(expected[1]));
            Assert.That(parser1.GetKeywords()[2].WhichType(), Is.EqualTo(expected[2]));
           
        }
        
        
        /// <summary>
        /// Check if the parser works correctly. Make examples and see if you can find problems with the code.
        /// Literals represent KeywordPairs with different Keywords
        /// A B C D
        /// Rating 2
        /// </summary>
        [Test]
        [Category("ShopParser")]
        ///// This test checks that the ValidateFindings method of ShopParser.cs returns true when given a valid array of KeywordPair objects.
        ///i.e If none of the keywords are repeated.

        public void Parsing_ValidFindings_True(){
             KeywordPair [] findings = new KeywordPair[4]{
                new KeywordPair(keyword1,"First"),
                new KeywordPair(keyword2,"Second"),
                new KeywordPair(keyword3,"Third"),
                new KeywordPair(intKeyword,"Fourth"),
            };

            Assert.IsTrue(ShopParser.ValidateFindings(findings));
        }
        
        /// <summary>
        /// Check if the parser works correctly. This time you should check if repetition invalidates the findings.
        /// A A B B C C
        /// Rating 2
        /// </summary>
        [Test]
        [Category("ShopParser")]
        ///// This test checks that the ValidateFindings method of ShopParser.cs returns false when given an invalid array of KeywordPair objects.
        ///i.e If the all keywords are repeated even when the their accompanying value are changed 
        public void Parsing_InvalidatedFindingsWithRepeatedEntry_False(){
             KeywordPair [] findings = new KeywordPair[6]{
                new KeywordPair(keyword1,"First"),
                new KeywordPair(keyword1,"Second"),
                new KeywordPair(keyword2,"Third"),
                new KeywordPair(keyword2,"Fourth"),
                new KeywordPair(keyword3,"Fifth"),
                new KeywordPair(keyword3,"Sixth")};

                Assert.IsFalse(ShopParser.ValidateFindings(findings));
                }
        
        /// <summary>
        /// Check if the parser works correctly. This time with circular keywords.
        /// A B C A
        /// Rating 2
        /// </summary>
        [Test]
        [Category("ShopParser")]
        ///// This test checks that the ValidateFindings method of ShopParser.cs returns false when given an invalid array of KeywordPair objects.
        ///i.e If only one of the  keywords are repeated at the end. Even when the its accompanying value is changed 
        public void Parsing_InvalidatedFindingsCircular_False(){
             KeywordPair [] findings = new KeywordPair[4]{
                new KeywordPair(keyword1,"First"),
                new KeywordPair(keyword2,"Second"),
                new KeywordPair(keyword3,"Third"),
                new KeywordPair(keyword1,"Fourth"),
            };

            Assert.IsFalse(ShopParser.ValidateFindings(findings));
        }
        
        /// <summary>
        /// See Tagfile (SampleOrder.tag) for more information. Are the correct number of keywords recognized ? 
        /// Rating 1
        /// </summary>
        [Test]
        [Category("ShopParser")]
        /// This test ensures that the ExtractFromTAG method of ShopParser returns an array with the correct number of entries
        /// When give a parser and a list of keywords

        public void Parsing_KeywordsSetTagString_CorrectNumberOfEntries(){
            
            int expected = 5;
            Assert.That(ShopParser.ExtractFromTAG(parser2,tag), Has.Length.EqualTo(expected));
        }
        
        /// <summary>
        /// Again consult the Tagfile for more information. The parsing should follow the order of the keywords you provided.
        /// Make sure to make it adaptable to different configurations.
        /// Rating 2
        /// </summary>
        [Test]
        [Category("ShopParser")]
        ////This test verifies that the ExtractFromTAG method of ShopParser returns an array of key:value pairs in the right order.
        public void Parsing_KeywordsSetTagString_ListOfProvidedTagsInOrder(){
            var result = ShopParser.ExtractFromTAG(parser2,tag);
            Assert.That(result[0].Key.GetString, Is.EqualTo("ItemNumber"));
            Assert.That(result[0].Value, Is.EqualTo("1"));
            Assert.That(result[1].Key.GetString, Is.EqualTo("ItemName"));
            Assert.That(result[1].Value, Is.EqualTo("Burger"));
            Assert.That(result[2].Key.GetString, Is.EqualTo("CustomerName"));
            Assert.That(result[2].Value, Is.EqualTo("James T. Kirk"));
            Assert.That(result[3].Key.GetString, Is.EqualTo("AmountOrdered"));
            Assert.That(result[3].Value, Is.EqualTo("2"));
            Assert.That(result[4].Key.GetString, Is.EqualTo("NetPrice"));
            Assert.That(result[4].Value, Is.EqualTo("8.00"));
            
        }

        /// <summary>
        /// Test if the VAT is calculated correctly for the Customer.CalculatePrice
        /// Rating 1
        /// </summary>
        [Test]
        [Category("Customer")]
        /// // This test checks that the CalculatePrice method of Customer.cs for a normal customer correctly adds the value-added tax (VAT).
        public void Invoice_CalculateNormalCustomer_AddValueAddedTax(){
            decimal expected = 119;
            Assert.That(customer1.CalculatePrice(100), Is.EqualTo(expected));
        }
        
        /// <summary>
        /// Test if the function CreateCustomer returns a customer
        /// Rating 1
        /// </summary>
        [Test]
        [Category("Customer")]
        // This test verifies that the CreateCustomer method of Customer.cs correctly creates a Customer object with the name specified.
        public void Invoice_CreateCustomer_ReturnsCustomer(){
            string expected = "James";
            Customer trial = Customer.CreateCustomer("James");
            Assert.That(trial.Name,Is.EqualTo(expected));
        }
        
        /// <summary>
        /// Test if the InvoicePosition.Price calculates correctly:
        /// Provided Orders, NetPrice is set.
        /// Rating 1
        /// </summary>
        [Test]
        [Category("Invoice")]

        ///// This test checks that the Price method of the InvoicePosition.cs calculates the correct total price

        public void Invoice_OrdersAndNetPriceValid_CalculateCorrectPrice(){
            decimal expected = 238;
            Assert.That(invoice1.Price(),Is.EqualTo(expected));
        }

        /// <summary>
        /// Test if IsValid number works 
        /// </summary>
        [Test]
        [Category("isValidNumber")]

        public void IsvalidNumber()
        {   int validInt = 120;
            string validString = validInt.ToString();
            string notValid = "m4523";
            string notvalidString = notValid.ToString();
            Assert.True(InvoicePosition.IsValidNumber(validString));
            Assert.False(InvoicePosition.IsValidNumber(notvalidString));
        }

        /// <summary>
        /// Test if IsValid number works 
        /// </summary>
        [Test]
        [Category("isDecimal")]

        public void isDecimal()
        {   decimal validDecimal = 353.1230m;
            string validString = validDecimal.ToString();
            Console.Write(validString);

            Assert.True(InvoicePosition.IsValidDecimal(validString));
           
        }
    }
}