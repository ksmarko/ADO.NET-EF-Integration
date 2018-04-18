using BLL.Interfaces;
using BLL.Services;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class ConnectionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IShopService>().To<ShopService>();
        }
    }
}
