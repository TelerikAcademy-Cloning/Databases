using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Superheroes.Data;
using Superheroes.Repositories;
using Superheroes.Repositories.Contracts;
using Superheroes.Services;
using Superheroes.Services.Contracts;

namespace Superheroes.CmdClient.Config
{
    public class AutofacConfig
    {
        public AutofacConfig()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SuperheroesDbContext>()
                .As<DbContext>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(EfGenericRepository<>))
                .As(typeof(IRepository<>));

            builder.RegisterType<EfUnitOfWork>()
                .As<IUnitOfWork>();

            builder.RegisterType<SuperheroesService>()
                .As<ISuperheroesService>();

            this.Container = builder.Build();
        }

        public IContainer Container { get; set; }
    }
}
