using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace CommandLineClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Example.CompositeDataServiceContainer exampleService = new Example.CompositeDataServiceContainer(new Uri("http://localhost:53282/service.svc"));

            /*
            foreach (var product in exampleService.Products)
                Console.WriteLine(product.Name + " (" + product.Price + ")");

            foreach (var productLine in exampleService.ProductLines)
                Console.WriteLine(productLine.Price);

            foreach (var user in exampleService.Users)
                Console.WriteLine(user.Username);

            foreach (var userRole in exampleService.Roles)
                Console.WriteLine(userRole.Name);

            exampleService.AddToUsers(new Example.User() { Username = "name", Password = "password", RoleId = 1 });
            exampleService.SaveChanges();

            exampleService.AddToProducts(new Example.Product() { Name = "New Product", Price = 45.5M });
            exampleService.SaveChanges();*/
        }
    }
}
