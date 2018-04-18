using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Configuration;

namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            BLL.Infrastructure.AutoMapperConfig.Initialize();

            Console.WriteLine("Select DB: 0 - EF, 1 - ADO");
            int selection = Convert.ToInt32(Console.ReadLine());
            NinjectModule serviceModule = new ServiceModule(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 
                selection == 0? StorageContext.EF : StorageContext.ADO);
            NinjectModule module = new ConnectionModule();
            var kernel = new StandardKernel(serviceModule, module);

            kernel.Get(typeof(Store));

            Console.ReadKey();
        }
    }
}
