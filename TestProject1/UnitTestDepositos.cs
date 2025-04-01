using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;


namespace TestProject1
{
    public class UnitTests3
    {
        private DepositoController _controller;
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

            _controller = new DepositoController(_context);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task PostDeposito_CreatesNewDeposito()
        {
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();
            
            var depositoDto = new DepositoDto
            {
                AtivoId = ativo.Id,
                Banco = "Banco A",
                Montante = 5000,
                NumeroConta = "123456789",
                TaxaJuros = 3.5,
                Titulares = new List<string> { "Titular 1" }
            };
            var result = await _controller.PostDeposito(depositoDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }

        [Test]
        public async Task PutDeposito_UpdatesExistingDeposito()
        {
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();
            
            var deposito = new Deposito
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Banco = "Banco A",
                Montante = 5000,
                NumeroConta = "123456789",
                TaxaJuros = 3.5,
                Titulares = new List<string> { "Titular 1" }
            };
            _context.Depositos.Add(deposito);
            _context.SaveChanges();

            var depositoDto = new DepositoDto
            {
                Id = deposito.Id,
                AtivoId = deposito.AtivoId,
                Banco = "Banco B",
                Montante = 6000,
                NumeroConta = deposito.NumeroConta,
                TaxaJuros = 4.0,
                Titulares = deposito.Titulares
            };
            var result = await _controller.PutDeposito(deposito.Id, depositoDto);

            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedDeposito = await _context.Depositos.FindAsync(deposito.Id);
            Assert.AreEqual(6000, updatedDeposito.Montante);
            Assert.AreEqual("Banco B", updatedDeposito.Banco);
        }

        [Test]
        public async Task DeleteDeposito_RemovesDeposito()
        {
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",  
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();
            
            var deposito = new Deposito
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Banco = "Banco A",
                Montante = 5000,
                NumeroConta = "123456789",
                TaxaJuros = 3.5,
                Titulares = new List<string> { "Titular 1" }
            };
            _context.Depositos.Add(deposito);
            _context.SaveChanges();

            var result = await _controller.DeleteDeposito(deposito.Id);
            Assert.IsInstanceOf<NoContentResult>(result);

            var deletedDeposito = await _context.Depositos.FindAsync(deposito.Id);
            Assert.IsNull(deletedDeposito);
        }

        [Test]
        public async Task GetDepositos_ReturnsList()
        {
            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Ação",
                DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                TaxaPercentagem = 5.0
            };

            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync(); 

            var deposito = new Deposito
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Banco = "Banco A",
                Montante = 5000,
                NumeroConta = "123456789",
                TaxaJuros = 3.5,
                Titulares = new List<string> { "Titular 1" }
            };

            _context.Depositos.Add(deposito);
            await _context.SaveChangesAsync(); 

  
            var result = await _controller.GetDepositos();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var depositos = okResult?.Value as List<DepositoDto>;

            Assert.IsNotNull(depositos);
            Assert.IsTrue(depositos.Count > 0); 
        }

        [Test]
        public async Task GetDeposito_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetDeposito(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}