using BLL.Interfaces;
using System;
using System.Collections.Generic;

namespace PL
{
    public class Store
    {
        readonly IShopService productService;

        public Store(IShopService service)
        {
            this.productService = service;
            Run();
        }

        public void Run()
        {
            Console.Clear();

            Print(productService.GetProducts(), "List of products:");
            Print(productService.GetProvidersForCategory(3), "List of clothes providers");
            Print(productService.GetProviderProducts(1), "List of Apple products");
            Print(productService.FindProduct(x => x.Price <= 20000), "List of products with price <= 20000:");
            Print(new List<string>(), "List of providers from Ukraine:");

            foreach (var el in productService.GetByLocation("Ukraine"))
                Console.WriteLine(el);
        }

        private void Print<T>(IEnumerable<T> list, string caption) where T : class
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + caption + "\n");
            Console.ResetColor();

            foreach (var el in list)
                Console.WriteLine(el);
        }
    }
}
