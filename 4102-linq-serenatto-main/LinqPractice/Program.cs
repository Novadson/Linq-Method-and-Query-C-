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

            List<Product> productPropetyTypeOfProductOnly = productList.OfType<Product>().ToList();

            foreach (var product in productPropetyTypeOfProductOnly)
            {
                Console.WriteLine($"Name: {product.Name} | Prie: {product.Price} |" +
                    $" StockQuantity: {product.StockQuantity} | Category: {product.Category}");
            }

            Console.WriteLine("----------------------------------------------------------------");

            Console.WriteLine("SORT PRODUCTS BY PRICE AND THEN BY NAME");

            //Sort products by price and then by name

            List<Product> productsOrderByPriceThenByName = productList.OrderBy(p => p.Price).ThenBy(p => p.Name).ToList();

            foreach (var product in productsOrderByPriceThenByName)
            {
                Console.WriteLine($"Name: {product.Name} | Prie: {product.Price} |" +
                    $" StockQuantity: {product.StockQuantity} | Category: {product.Category}");
            }


            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
