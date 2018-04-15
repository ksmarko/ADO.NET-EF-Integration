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
            Bind<IShopService>().To<ShopService>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Select DB: 0 - EF, 1 - ADO");
            //int selection = Convert.ToInt32(Console.ReadLine());
            BLL.Infrastructure.AutoMapperConfig.Initialize();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection", /*selection == 0? StorageContext.EF : */StorageContext.ADO);
            NinjectModule module = new ConnModule();
            var kernel = new StandardKernel(serviceModule, module);
            kernel.Get(typeof(Store));

            Console.ReadKey();
        }
    }
}
