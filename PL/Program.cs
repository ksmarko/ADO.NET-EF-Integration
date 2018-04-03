using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Exceptions;

namespace PL
{
    public class ConnModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IProductService>().To<ProductService>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BLL.Infrastructure.AutoMapperConfig.Initialize();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection", StorageContext.EF);
            NinjectModule module = new ConnModule();
            var kernel = new StandardKernel(serviceModule, module);
            kernel.Get(typeof(Store));

            Console.ReadKey();
        }
    }
}
