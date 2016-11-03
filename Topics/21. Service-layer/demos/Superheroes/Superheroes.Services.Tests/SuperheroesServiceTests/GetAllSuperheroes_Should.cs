using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Superheroes.Models;
using Superheroes.Repositories.Contracts;
using Telerik.JustMock;

namespace Superheroes.Services.Tests.SuperheroesServiceTests
{
    [TestClass]
    public class GetAllSuperheroes_Should
    {
        private IRepository<Superhero> mockSuperheroes;
        private IRepository<City> mockCities;
        private IRepository<Power> mockPowers;
        private IUnitOfWork mockUnitOfWork;

        public GetAllSuperheroes_Should()
        {
            this.mockSuperheroes = Mock.Create<IRepository<Superhero>>();
            this.mockCities = Mock.Create<IRepository<City>>();
            this.mockPowers = Mock.Create<IRepository<Power>>();
            this.mockUnitOfWork = Mock.Create<IUnitOfWork>();
        }

        [TestMethod]
        public void GetAllSuperheroes_WhenThereAreSuperheroes()
        {
            var superherosList = new List<Superhero>()
                .AsQueryable();
            Mock.Arrange(() => this.mockSuperheroes.Entities)
                .Returns(superherosList);

            var service = new SuperheroesService(this.mockUnitOfWork, this.mockSuperheroes, this.mockPowers, this.mockCities);

            var superheroes = service.GetAllSuperheroes()
                .ToList();

            CollectionAssert.AreEqual(superherosList.ToList(), superheroes);
        }
    }
}
