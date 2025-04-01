using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject1
{
    public class CarteirasTests
    {
        private CarteiraController _controller;
        private Dbes2 _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Dbes2>()
                .UseNpgsql("Host=localhost;Database=es2_test;Username=postgres;Password=es2")
                .Options;

            _context = new Dbes2(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new CarteiraController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetCarteiras_ReturnsList()
        {
            var utilizador = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Test User",
                Email = "test@example.com",
                Senha = "password123",
                Cargo = "Investor",
                PermissaoId = null 
            };
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();
            
            _context.Carteiras.Add(new Carteira
            {
                Id = Guid.NewGuid(),
                UtilizadorId = utilizador.Id,
                AtivoId =  ativo.Id,
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                Montante = 1000
            });

            await _context.SaveChangesAsync();

            var result = await _controller.GetCarteiras();

            Assert.IsInstanceOf<ActionResult<IEnumerable<CarteiraDto>>>(result);
            var okResult = result.Result as OkObjectResult;
            var carteiras = okResult?.Value as List<CarteiraDto>;
            Assert.IsNotNull(carteiras);
            Assert.That(carteiras, Has.Count.GreaterThan(0));
        }

        [Test]
        public async Task GetCarteira_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetCarteira(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostCarteira_CreatesNewCarteira()
        {
            var utilizador = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Test User",
                Email = "test@example.com",
                Senha = "password123",
                Cargo = "Investor",
                PermissaoId = null 
            };
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();


            var dataInicio = DateOnly.FromDateTime(DateTime.UtcNow);
            var carteiraDto = new CarteiraDto
            {
                UtilizadorId = utilizador.Id,
                AtivoId = ativo.Id, 
                DataInicio = dataInicio.ToDateTime(TimeOnly.MinValue),
                Montante = 1000
            };

            var result = await _controller.PostCarteira(carteiraDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }


        [Test]
        public async Task PutCarteira_UpdatesExistingCarteira()
        {
            var utilizador = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Test User",
                Email = "test@example.com",
                Senha = "password123",
                Cargo = "Investor",
                PermissaoId = null 
            };
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();


            var carteira = new Carteira
            {
                Id = Guid.NewGuid(),
                UtilizadorId = utilizador.Id,
                AtivoId = ativo.Id,
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                Montante = 1000
            };
            _context.Carteiras.Add(carteira);
            await _context.SaveChangesAsync();

            var carteiraDto = new CarteiraDto
            {
                Id = carteira.Id,
                UtilizadorId = carteira.UtilizadorId,
                AtivoId = carteira.AtivoId,
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow).ToDateTime(TimeOnly.MinValue),
                Montante = 2000
            };

            var result = await _controller.PutCarteira(carteira.Id, carteiraDto);
            Assert.IsInstanceOf<NoContentResult>(result);

            var updatedCarteira = await _context.Carteiras.FindAsync(carteira.Id);
            Assert.AreEqual(2000, updatedCarteira.Montante);
        }


        [Test]
        public async Task DeleteCarteira_RemovesCarteira()
        {
            var utilizador = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Test User",
                Email = "test@example.com",
                Senha = "password123",
                Cargo = "Investor",
                PermissaoId = null 
            };
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var carteira = new Carteira
            {
                Id = Guid.NewGuid(),
                UtilizadorId = utilizador.Id,
                AtivoId = ativo.Id,
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                Montante = 1000
            };
            _context.Carteiras.Add(carteira);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteCarteira(carteira.Id);
            Assert.IsInstanceOf<NoContentResult>(result);

            var deletedCarteira = await _context.Carteiras.FindAsync(carteira.Id);
            Assert.IsNull(deletedCarteira);
        }
    }
}
