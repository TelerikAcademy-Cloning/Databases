using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Superheroes.Models;
using Superheroes.Repositories.Contracts;
using Telerik.JustMock;

namespace Superheroes.Services.Tests.SuperheroesServiceTests
{
    [TestClass]
    public class Create_Should
    {
        private IRepository<Superhero> mockSuperheroes;
        private IRepository<City> mockCities;
        private IRepository<Power> mockPowers;
        private IUnitOfWork mockUnitOfWork;

        public Create_Should()
        {
            mockSuperheroes = Mock.Create<IRepository<Superhero>>();
            mockCities = Mock.Create<IRepository<City>>();
            mockPowers = Mock.Create<IRepository<Power>>();
            mockUnitOfWork = Mock.Create<IUnitOfWork>();
        }

        [TestMethod]
        public void CreateNewSupehero_WhenCitiesListIsNull()
        {
            // Arrange
            var service = new SuperheroesService(mockUnitOfWork, mockSuperheroes, mockPowers, mockCities);

            bool isAddCalled = false;

            Mock.Arrange(() => this.mockSuperheroes.Add(Arg.IsAny<Superhero>()))
                .DoInstead(() => isAddCalled = true);

            // Act
            service.CreateSuperhero("Superman", "Clark Kent", "Metropolis", null);

            // Assert
            Assert.IsTrue(isAddCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowArgumentException_WhenSuperheroNameIsLessThan3Characters()
        {
            //Arrange
            var service = new SuperheroesService(mockUnitOfWork, mockSuperheroes, mockPowers, mockCities);
            var superheroName = this.GetRandomString(2);

            //Act && Assert
            service.CreateSuperhero("SU", "Clark Kent", "Metropolis", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThrowArgumentException_WhenSuperheroNameIsMoreThan20Characters()
        {
            //Arrange
            var service = new SuperheroesService(mockUnitOfWork, mockSuperheroes, mockPowers, mockCities);

            var superheroName = this.GetRandomString(21);
            //Act && Assert
            service.CreateSuperhero(superheroName, "Clark Kent", "Metropolis", null);
        }

        [TestMethod]
        public void CreateNewSuperhero_WhenDataIsValid()
        {
            //Arrange
            bool isCalled = false;

            Mock.Arrange(() => this.mockSuperheroes.Add(Arg.IsAny<Superhero>()))
                .DoInstead(() => isCalled = true);

            var service = new SuperheroesService(mockUnitOfWork, mockSuperheroes, mockPowers, mockCities);

            //Act
            service.CreateSuperhero("Superman", "Clark Kent", "Metropolis", new List<string>());

            //Assert
            Assert.IsTrue(isCalled);
        }

        private string GetRandomString(int length)
        {
            var stringBuilder = new StringBuilder();

            while (stringBuilder.Length < length)
            {
                stringBuilder.Append(Guid.NewGuid());
            }

            return stringBuilder.ToString().Substring(0, length);
        }
    }
}
