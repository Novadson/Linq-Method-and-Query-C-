using LinqPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    public static class DAO
    {


        public static List<Product> getProductsOne()
        {

            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1000, StockQuantity = 10 },
                new Product { Id = 2, Name = "Headphones", Category = "Electronics", Price = 100, StockQuantity = 50 },
                new Product { Id = 3, Name = "Keyboard", Category = "Electronics", Price = 30, StockQuantity = 100 },
                new Product { Id = 4, Name = "Desk Chair", Category = "Furniture", Price = 150, StockQuantity = 20 },
                new Product { Id = 5, Name = "Notebook", Category = "Stationery", Price = 5, StockQuantity = 200 },
            };

        }
        public static List<Product> getProductsTwo()
        {

            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1000, StockQuantity = 10 },
                new Product { Id = 2, Name = "Headphones", Category = "Electronics", Price = 100, StockQuantity = 50 },
                new Product { Id = 3, Name = "Keyboard", Category = "Electronics", Price = 30, StockQuantity = 100 },
                new Product { Id = 4, Name = "Desk Chair", Category = "Furniture", Price = 150, StockQuantity = 20 },
                new Product { Id = 5, Name = "Notebook", Category = "Stationery", Price = 5, StockQuantity = 200 },
            };

        }


        public static List<Customer> getCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 1,
                            OrderDate = DateTime.Now.AddDays(-5),
                            TotalAmount = 1100,
                            Items = new List<OrderItem>
                            {
                                new OrderItem { OrderId =1, ProductId = 1, Quantity = 1, Price = 1000 },
                                new OrderItem { OrderId =1, ProductId = 2, Quantity = 1, Price = 100 }
                            }
                        }
                    }
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            Id = 2,
                            OrderDate = DateTime.Now.AddDays(-2),
                            TotalAmount = 180,
                            Items = new List<OrderItem>
                            {
                                new OrderItem { OrderId =2, ProductId = 4, Quantity = 1, Price = 150 },
                                new OrderItem { OrderId =2, ProductId = 3, Quantity = 1, Price = 30 }
                            }
                        }
                    }
                }
            };
        }

    }
}
