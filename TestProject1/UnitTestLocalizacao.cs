using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;

namespace TestProject1
{
    public class LocalizacoesControllerTests
    {
        private LocalizacoesController _controller;
        private Dbes2 _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Dbes2>()
                .UseNpgsql(TestConfig.ConnectionString)
                .Options;

            _context = new Dbes2(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new LocalizacoesController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetLocalizacoes_ReturnsList()
        {
            _context.Localizacoes.Add(new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            });
            await _context.SaveChangesAsync();

            var result = await _controller.GetLocalizacoes();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var localizacoes = okResult.Value as List<LocalizacoDto>;
            Assert.IsNotNull(localizacoes);
            Assert.IsTrue(localizacoes.Count > 0);
        }

        [Test]
        public async Task GetLocalizaco_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetLocalizaco(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostLocalizaco_CreatesNewLocalizaco()
        {
            var localizacoDto = new LocalizacoDto
            {
                Endereco = "Rua B",
                Cidade = "Cidade B",
                Pais = "País B",
                CodigoPostal = "67890"
            };

            var result = await _controller.PostLocalizaco(localizacoDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }

        [Test]
        public async Task PutLocalizaco_UpdatesExistingLocalizaco()
        {
            var localizaco = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizaco);
            await _context.SaveChangesAsync();

            var localizacoDto = new LocalizacoDto
            {
                Id = localizaco.Id,
                Endereco = "Rua C",
                Cidade = "Cidade C",
                Pais = "País C",
                CodigoPostal = "54321"
            };

            var result = await _controller.PutLocalizaco(localizaco.Id, localizacoDto);

            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedLocalizaco = await _context.Localizacoes.FindAsync(localizaco.Id);
            Assert.AreEqual("Rua C", updatedLocalizaco.Endereco);
        }

        [Test]
        public async Task DeleteLocalizaco_RemovesLocalizaco()
        {
            var localizaco = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizaco);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteLocalizaco(localizaco.Id);

            Assert.IsInstanceOf<NoContentResult>(result);
            var deletedLocalizaco = await _context.Localizacoes.FindAsync(localizaco.Id);
            Assert.IsNull(deletedLocalizaco);
        }
    }
}
