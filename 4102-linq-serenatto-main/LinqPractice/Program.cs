using LinqPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Seed data
            List<Product> productList = DAO.getProducts();
            List<Customer> customersList = DAO.getCustomers();

            List<Order> ordersList = [.. DAO.getCustomers().SelectMany(x => x.Orders)];// using collection expression

            //Filter products with a price greater than 50.
            Console.WriteLine("FILTER PRODUCTS WITH A PRICE GREATER THAN 50");


            List<Product> productsGreaterThanFiftyMethod = productList.Where(x => x.Price > 50).ToList();// syntax linq method

            List<Product> productsGreaterThanFiftyQuery = (from p in productList // syntax linq query
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

            List<Product> productPropetyTypeOfProductOnlyMethod = productList.OfType<Product>().ToList(); //systax linq method

            List<Product> productPropetyTypeOfProductOnlyQuery = (from p in productList //systax linq query
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

            List<Product> productsOrderByPriceThenByNameMethod = productList.OrderBy(p => p.Price).ThenBy(p => p.Name).ToList(); //systax linq method

            List<Product> productsOrderByPriceThenByNameQuery = [.. from p in productList // syntax query using collection expression
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

            List<IGrouping<string, Product>> productsGruopByCategoryMethod = productList.GroupBy(p => p.Category).ToList();// syntax method

            List<IGrouping<string, Product>> productsGruopByCategoryQuery = [.. from p in productList //syntax query using experssion collection
                                                                            group p by p.Category];

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

            List<IGrouping<string, Product>>? productsToLookUpCategoryMethod = [.. productList.ToLookup(p => p.Category)];// syntax method ToLookUp is the as GroupBy but it execute immediately.

            foreach (var category in productsToLookUpCategoryMethod)
            {
                Console.WriteLine($"Product category:{category.Key}");

                foreach (var product in category)
                    Console.WriteLine($"Name: {product.Name} | Price: {product.Price} | StockQuantity:{product.StockQuantity}");
            }

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

            List<string> productsNameMethod = [.. productList.Select(p => p.Name)]; // syntax method using collection expression

            List<string> productsNameQuery = [.. from product in productList // syntax query using collection expression
                                                 select product.Name];

            Console.WriteLine("Product names:");
            productsNameMethod.ForEach(p => Console.WriteLine(p));

            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
