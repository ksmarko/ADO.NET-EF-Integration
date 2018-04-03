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
        readonly IService service;

        public Store(IService service)
        {
            this.service = service;

            Run();
        }

        public void Run()
        {
            foreach (var el in service.GetAll())
                Console.WriteLine(el);
        }
    }
}
