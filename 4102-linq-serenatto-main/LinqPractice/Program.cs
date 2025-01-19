using LinqPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.X86;

namespace LinqPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Seed data
            List<Product> productListOne = DAO.getProductsOne();
            List<Product> productListTwo = DAO.getProductsTwo();
            List<Customer> customersList = DAO.getCustomers();
            IsPrimers("racecar");

            var exceptList = productListOne.Except(productListTwo);

            var words = Dictionaries.GetWords();

            List<Order> ordersList = [.. DAO.getCustomers().SelectMany(x => x.Orders)];// using collection expression

            List<OrderItem> OrderItemList = [.. ordersList.SelectMany(item => item.Items)];// using collection expression

            //Filter products with a price greater than 50.
            Console.WriteLine("FILTER PRODUCTS WITH A PRICE GREATER THAN 50");


            List<Product> productsGreaterThanFiftyMethod = productListOne.Where(x => x.Price > 50).ToList();// syntax linq method

            List<Product> productsGreaterThanFiftyQuery = (from p in productListOne // syntax linq query
                                                           where p.Price > 50
                                                           select p).ToList();

            foreach (var product in productsGreaterThanFiftyMethod)
            {
                Console.WriteLine($"Name: {product.Name} | Prie: {product.Price} |" +
                    $" StockQuantity: {product.StockQuantity} | Category: {product.Category}");
            }

            Console.WriteLine("----------------------------------------------------------------");

            //filter elements of a specific type

            Console.WriteLine("PRODUCT FILTER ELEMENTS OF A SPECIFIC TYPE");

            List<Product> productPropetyTypeOfProductOnlyMethod = productListOne.OfType<Product>().ToList(); //systax linq method

            List<Product> productPropetyTypeOfProductOnlyQuery = (from p in productListOne //systax linq query
                                                                  where p is Product
                                                                  select (Product)p).ToList();

            foreach (var product in productPropetyTypeOfProductOnlyMethod)
            {
                Console.WriteLine($"Name: {product.Name} | Prie: {product.Price} |" +
                    $" StockQuantity: {product.StockQuantity} | Category: {product.Category}");
            }

            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("SORT PRODUCTS BY PRICE AND THEN BY NAME");

            //Sort products by price and then by name

            List<Product> productsOrderByPriceThenByNameMethod = productListOne.OrderBy(p => p.Price).ThenBy(p => p.Name).ToList(); //systax linq method

            List<Product> productsOrderByPriceThenByNameQuery = [.. from p in productListOne // syntax query using collection expression
                                                                orderby p.Price, p.Name
                                                                select p];

            foreach (var product in productsOrderByPriceThenByNameMethod)
            {
                Console.WriteLine($"Name: {product.Name} | Prie: {product.Price} |" +
                    $" StockQuantity: {product.StockQuantity} | Category: {product.Category}");
            }


            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("GROUP PRODUCTS BY CATEGORY");

            //group products by category.

            List<IGrouping<string, Product>> productsGruopByCategoryMethod = productListOne.GroupBy(p => p.Category).ToList();// syntax method

            var productsGruopByCategoryQuery = from p in productListOne //syntax query using experssion collection
                                               group p by p.Category into productCollection
                                               where productCollection.Count() > 0
                                               select new
                                               {
                                                   product = productCollection.Key,
                                                   names = productCollection.Select(p => p.Name)

                                               };


            foreach (var product in productsGruopByCategoryQuery)
            {
                Console.WriteLine($"Category:{product.product}");
                Console.WriteLine(" ");
                Console.WriteLine(string.Join(",", product.names));
            }

            Console.WriteLine();

            // List<IGrouping<string, Product>> 


            foreach (var item in productsGruopByCategoryMethod)
            {
                Console.WriteLine($"Product category: {item.Key}");
                foreach (var prodcut in item)
                {
                    Console.WriteLine($"Name: {prodcut.Name} | Prie: {prodcut.Price} |" +
                        $" StockQuantity: {prodcut.StockQuantity} | Category: {prodcut.StockQuantity}");
                }
            }

            //create a lookup dictionary by category.
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("CREATE A LOOKUP DICTIONARY BY CATEGORY");

            List<IGrouping<string, Product>>? productsToLookUpCategoryMethod = [.. productListOne.ToLookup(p => p.Category)];

            foreach (var category in productsToLookUpCategoryMethod)
            {
                Console.WriteLine($"Product category:{category.Key}");

                foreach (var product in category)
                    Console.WriteLine($"Name: {product.Name} | Price: {product.Price} | StockQuantity:{product.StockQuantity}");
            }

            ////one for a List of only words which have duplicates

            var duplicatesWords = words.ToLookup(p => p.Key)
                                       .Where(p => p.Key.Count() > 1)
                                       .Select(p => p).ToList();

            ////one for an DTO Array of each word and populate the 

            var duplicatesWordsDTO = words.ToLookup(p => p.Key)
                                            .Where(g => g.Key.Count() > 1)
                                            .Select(g => new Dictionaries.DuplicatesDTO
                                            {
                                                Word = g.Key,
                                                Count = g.Count()

                                            }).ToArray();


            Console.WriteLine(string.Join(",", words.Keys.DistinctBy(k => k)));
            //join customers with their orders.
            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("JOIN CUSTOMERS WITH THEIR ORDERS");

            List<Customer> customersWithOrdersMethod = customersList.Join(ordersList, // syntax method
                                                 customer => customer.Id,
                                                 order => order.Id,
                                                 (customer, order) => customer).ToList();

            List<Customer> customersWithOrdersQuery = [.. from customer in customersList //syntax query using collection expression
                                                      join order in ordersList
                                                      on customer.Id equals order.Id
                                                      join product in productListOne
                                                      on customer.Id equals product.Id
                                                      select customer];


            Console.WriteLine("JOIN CUSTOMERS WITH THEIR ORDERS");
            foreach (var customer in customersWithOrdersQuery)
            {
                Console.WriteLine($"Custome Name: {customer.Name} | Id: {customer.Id}");
                foreach (var order in customer.Orders)
                    Console.WriteLine($"Order Id: {order.Id} | TotalAmount:" +
                        $" {order.TotalAmount} | OrderDate: {order.OrderDate}");
            }

            //groupJoin() to group customers with their orders.
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("GROUPJOIN CUSTOMERS WITH THEIR ORDERS");

            var customersWithOrdersGroupJoinQuery = from customer in customersList
                                                    join order in ordersList
                                                    on customer.Id equals order.Id
                                                    into collectionGruop
                                                    select new
                                                    {
                                                        Customer = customer,
                                                        Orders = collectionGruop
                                                    };


            foreach (var item in customersWithOrdersGroupJoinQuery)
            {
                Console.WriteLine($"Custome Name: {item.Customer.Name} | Id: {item.Customer.Id}");
                foreach (var order in item.Orders)
                    Console.WriteLine($"Order Id: {order.Id} | TotalAmount:" +
                        $" {order.TotalAmount} | OrderDate: {order.OrderDate}");
            }


            //Select() to project a list of product names.
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("PROJECT A LIST OF PRODUCT NAMES");

            List<string> productsNameMethod = [.. productListOne.Select(p => p.Name)]; // syntax method using collection expression

            List<string> productsNameQuery = [.. from product in productListOne // syntax query using collection expression
                                                 select product.Name];

            Console.WriteLine("Product names:");
            productsNameMethod.ForEach(p => Console.WriteLine(p));

            //

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("CHECK IF ALL PRODUCTS HAVE A STOCK QUANTITY GREATER THAN 0.");
            bool stockQuantityIsThanZero = productListOne.All(p => p.StockQuantity > 0);
            Console.WriteLine(stockQuantityIsThanZero);


            //Check if any product belongs to the "Furniture" category.
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("CHECK IF ANY PRODUCT BELONGS TO THE FURNITURE CATEGORY.");

            bool productBelongsFurniture = productListOne.Any(p => p.Category
                                                     .Equals("FURNITURE",
                                                     StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(productBelongsFurniture);

            //Check if a specific product exists in the list like "Laptop".

            Product? prodMethod = productListOne.FirstOrDefault(p => p.Name
                                                      .Contains("Laptop", StringComparison.OrdinalIgnoreCase));

            Product? prodQuery = (from p in productListOne
                                  where p.Name.Contains("Laptop", StringComparison.OrdinalIgnoreCase)
                                  select p).FirstOrDefault();

            Console.WriteLine($"Product: {prodMethod?.Name}");

            //Create a string of product names separated by commas.
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("CREATE A STRING OF PRODUCT NAMES SEPARATED BY COMMAS.");

            string? str = productListOne.Select(p => p.Name)
                                 .Aggregate((nameOne, nameTow) => nameOne + "," + nameTow);

            //Use Average(), Count(), Max(), and Sum() on the product prices or stock quantities.

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("USE AVERAGE(), COUNT(), MAX(), AND SUM() ON THE PRODUCT PRICES OR STOCK QUANTITIES");

            var resProduct = productListOne.ToLookup(p => p.Name)
                                        .Where(p => p.Any())
                                        .OrderBy(p => p.Max(p => p.Price))
                                        .Select(p => new
                                        {
                                            Name = p.Key,
                                            StockQuantity = p.Sum(p => p.StockQuantity),
                                            AveragePrice = p.Average(p => p.Price),
                                            StockQuantityTotal = p.Count()
                                        }).ToList();

            resProduct.ForEach(p => Console.WriteLine($"Product name: {p.Name} | Stock Quantity: {p.StockQuantity}" +
               $" AveragePrice: {p.AveragePrice}| StockQuantityTotal: {p.StockQuantityTotal}"));

            //ElementAt(), ElementAtOrDefault(), First(), FirstOrDefault(), Last(),

            Console.WriteLine("PRODUCT AT POSITION 0 ElementAt(0)");
            Product? prodcutAt = productListOne.ElementAt(0);
            Console.WriteLine($"Name: {prodcutAt.Name} | Price: {prodcutAt.Price}");

            Console.WriteLine("PRODUCT AT POSITION 0 ElementAtOrDefault(1)");
            Product? prodcutAtOrDefault = productListOne.ElementAtOrDefault(1);
            Console.WriteLine($"Name: {prodcutAtOrDefault?.Name} | Price: {prodcutAtOrDefault?.Price}");

            Console.WriteLine("GET THE FIRST PRODUCT");
            Product? prodcutFirst = productListOne.First();
            Console.WriteLine($"Name: {prodcutFirst?.Name} | Price: {prodcutFirst?.Price}");

            Console.WriteLine("GET THE FIRSTORDEFAULT PRODUCT");
            Product? prodcutFirstOrDefault = productListOne.FirstOrDefault();
            Console.WriteLine($"Name: {prodcutFirstOrDefault?.Name} | Price: {prodcutFirstOrDefault?.Price}");


            Console.WriteLine("GET THE LAST PRODUCT");
            Product? prodcutLast = productListOne.Last();
            Console.WriteLine($"Name: {prodcutLast?.Name} | Price: {prodcutLast?.Price}");


            Console.WriteLine("GET THE LAST PRODUCT");
            Product? prodcutLastOrDefault = productListOne.LastOrDefault();
            Console.WriteLine($"Name: {prodcutLastOrDefault?.Name} | Price: {prodcutLastOrDefault?.Price}");

            //Use SequenceEqual() to compare two lists of products.
            bool verifyIfSequenceEqual = productListOne.SequenceEqual(productListTwo, new ProductEqualityComparer());
            Console.WriteLine(verifyIfSequenceEqual);

            //Use Concat() to combine two product lists.
            //TODO var concatLists = productListOne.Concat(productListTwo);

            //TODO Console.WriteLine(concatLists);

            //Use Range() to combine two product lists.
            //TODO productListOne.AddRange(productListTwo);


            //Use Range() with Except to combine two product lists.
            // TODO productListOne.AddRange(productListTwo.Except(productListOne));

            //Use Except() to combine two product lists.




            Console.WriteLine();



            Console.ReadKey();
        }
        public static bool IsPrimers(string palidrone)
        {


            var strToCharArray = palidrone.ToCharArray();

            var result = strToCharArray.SequenceEqual(strToCharArray.Reverse());

            if (result)
            {

            }

            return result;
        }


        public static bool IsPrimerNumber(int number)
        {
            if (number <= 1)
                return false;

            var Isp = !Enumerable.Range(2, (int)Math.Sqrt(number) - 1).Any(n => number % n == 0);
            return false;

        }
    }
    public class ProductEqualityComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                   x.Name == y.Name &&
                   x.Category == y.Category &&
                   x.Price == y.Price &&
                   x.StockQuantity == y.StockQuantity;
        }

        public int GetHashCode(Product obj)
        {
            return HashCode.Combine(obj.Id, obj.Name, obj.Category, obj.Price, obj.StockQuantity);
        }

    }

}
