using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Superheroes.CmdClient.Config;
using Superheroes.Data;
using Superheroes.Models;
using Superheroes.Repositories;
using Superheroes.Repositories.Contracts;
using Superheroes.Services;
using Superheroes.Services.Contracts;

namespace Superheroes.CmdClient
{
    public class StartUp
    {
        public static void Main()
        {
            AutofacConfig = new AutofacConfig();

            ISuperheroesService service = AutofacConfig.Container.Resolve<ISuperheroesService>();

            service.GetAllSuperheroes()
                .ToList()
                .ForEach(Console.WriteLine);
            Console.WriteLine("---------------------------------------------------------------------");

            service.CreateSuperhero("Nightwing", "Dick Grayson", "Gotham", "Utility belt", "Acrobat");

            service.GetAllSuperheroes()
                .ToList()
                .ForEach(Console.WriteLine);
            Console.WriteLine("---------------------------------------------------------------------");
        }

        public static AutofacConfig AutofacConfig { get; set; }
    }
}
