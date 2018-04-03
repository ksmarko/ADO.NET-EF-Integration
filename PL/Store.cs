using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Store
    {
        readonly IProductService productService;

        public Store(IProductService service)
        {
            this.productService = service;

            Run();
        }

        public void Run()
        {
            Console.WriteLine("List of products: ");

            foreach (var el in productService.GetProducts())
                Console.WriteLine(el);

            Console.WriteLine("\nList of Electronics providers: ");

            foreach (var el in productService.GetProvidersForCategory(productService.FindCategory(x => x.Id == 1).FirstOrDefault()))
                Console.WriteLine(el);

            Console.WriteLine("\nList of provider's products: ");

            foreach (var el in productService.GetProviderProducts(productService.FindProvider(x => x.Id == 1).FirstOrDefault()))
                Console.WriteLine(el);

            Console.WriteLine("\nList of products with price <= 30000: ");

            foreach (var el in productService.FindProduct(x => x.Price <= 30000))
                Console.WriteLine(el);

            Console.WriteLine("\nList of providers from Ukraine: ");

            foreach (var el in productService.GetByLocation("Ukraine"))
                Console.WriteLine(el.Name);

        }
    }
}
