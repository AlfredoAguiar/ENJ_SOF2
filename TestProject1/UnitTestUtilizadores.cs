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
    public class UtilizadoreControllerTests
    {
        private UtilizadoreController _controller;
        private Dbes2 _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Dbes2>()
                .UseNpgsql("Host=localhost;Database=es2_test;Username=postgres;Password=es2") // Database connection string
                .Options;
            _context = new Dbes2(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new UtilizadoreController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

         [Test]
        public async Task GetUtilizadores_ReturnsList()
        {
            var permissao = new Permisso
            {
                Id = Guid.NewGuid(),
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = false,
                PodeVerRegistos = true,
                PodeAlterarPermissoes = false
            };

            _context.Permissoes.Add(permissao);
            _context.SaveChanges();
            
            _context.Utilizadores.Add(new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "João Silva",
                Email = "joao.silva@email.com",
                Senha = "password123",
                Cargo = "Gerente",
                PermissaoId = permissao.Id
            });
            await _context.SaveChangesAsync();

            var result = await _controller.GetUtilizadores();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var utilizadores = okResult.Value as List<UtilizadoreDto>;
            Assert.IsNotNull(utilizadores);
            Assert.IsTrue(utilizadores.Count > 0);
        }

        [Test]
        public async Task GetUtilizadore_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetUtilizadore(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostUtilizadore_CreatesNewUtilizadore()
        {
            var permissao = new Permisso
            {
                Id = Guid.NewGuid(),
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = false,
                PodeVerRegistos = true,
                PodeAlterarPermissoes = false
            };

            _context.Permissoes.Add(permissao);
            _context.SaveChanges();
            
            var utilizadoreDto = new UtilizadoreDto
            {
                Nome = "Maria Santos",
                Email = "maria.santos@email.com",
                Senha = "password456",
                Cargo = "Analista",
                PermissaoId = permissao.Id
            };

            var result = await _controller.PostUtilizadore(utilizadoreDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }

        [Test]
        public async Task PutUtilizadore_UpdatesExistingUtilizadore()
        {
            var permissao = new Permisso
            {
                Id = Guid.NewGuid(),
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = false,
                PodeVerRegistos = true,
                PodeAlterarPermissoes = false
            };

            _context.Permissoes.Add(permissao);
            _context.SaveChanges();
            
            var utilizadore = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Carlos Pereira",
                Email = "carlos.pereira@email.com",
                Senha = "password789",
                Cargo = "Supervisor",
                PermissaoId = permissao.Id
            };
            _context.Utilizadores.Add(utilizadore);
            await _context.SaveChangesAsync();
            
            utilizadore = await _context.Utilizadores.FindAsync(utilizadore.Id); 

       
            var utilizadoreDto = new UtilizadoreDto
            {
                Id = utilizadore.Id,
                Nome = "Carlos Pereira",
                Email = "carlos.pereira@novoemail.com",
                Senha = "password789",
                Cargo = "Gerente", 
                PermissaoId = utilizadore.PermissaoId
            };

        
            utilizadore.Nome = utilizadoreDto.Nome;
            utilizadore.Email = utilizadoreDto.Email;
            utilizadore.Senha = utilizadoreDto.Senha;
            utilizadore.Cargo = utilizadoreDto.Cargo;
            
            _context.Entry(utilizadore).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            
            var updatedUtilizadore = await _context.Utilizadores.FindAsync(utilizadore.Id);
            Assert.AreEqual("Carlos Pereira", updatedUtilizadore.Nome);
            Assert.AreEqual("carlos.pereira@novoemail.com", updatedUtilizadore.Email);
            Assert.AreEqual("Gerente", updatedUtilizadore.Cargo);
        }


        [Test]
        public async Task DeleteUtilizadore_RemovesUtilizadore()
        {
            var permissao = new Permisso
            {
                Id = Guid.NewGuid(),
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = false,
                PodeVerRegistos = true,
                PodeAlterarPermissoes = false
            };

            _context.Permissoes.Add(permissao);
            _context.SaveChanges();
            
            var utilizadore = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = "Lucas Costa",
                Email = "lucas.costa@email.com",
                Senha = "password111",
                Cargo = "Coordenador",
                PermissaoId = permissao.Id
            };
            _context.Utilizadores.Add(utilizadore);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteUtilizadore(utilizadore.Id);

            Assert.IsInstanceOf<NoContentResult>(result);
            var deletedUtilizadore = await _context.Utilizadores.FindAsync(utilizadore.Id);
            Assert.IsNull(deletedUtilizadore);
        }
    }
}