using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;


namespace TestProject1
{
    public class UnitTests4
    {
   
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

           
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [TestFixture]
        public class FundosInvestimentoTests
        {
            private FundosInvestimentosController _controller;
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

                _controller = new FundosInvestimentosController(_context);
            }

            [TearDown]
            public void TearDown()
            {
                _context.Dispose();
            }

            [Test]
            public async Task PostFundosInvestimento_CreatesNewFundo()
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
                
                var fundoDto = new FundosInvestimentoDto
                {
                    AtivoId =ativo.Id,
                    Nome = "Fundo ABC",
                    DuracaoMeses = 12,
                    MontanteInvestido = 10000,
                    TaxaJurosPadrao = 5.5
                };
                var result = await _controller.PostFundosInvestimento(fundoDto);

                Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
                var createdResult = result.Result as CreatedAtActionResult;
                Assert.IsNotNull(createdResult);
            }

            [Test]
            public async Task PutFundosInvestimento_UpdatesExistingFundo()
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
                
                var fundo = new FundosInvestimento
                {
                    Id = Guid.NewGuid(),
                    AtivoId =ativo.Id,
                    Nome = "Fundo Inicial",
                    DuracaoMeses = 12,
                    MontanteInvestido = 10000,
                    TaxaJurosPadrao = 5.5
                };
                _context.FundosInvestimentos.Add(fundo);
                _context.SaveChanges();

                var fundoDto = new FundosInvestimentoDto
                {
                    Id = fundo.Id,
                    Nome = "Fundo Atualizado",
                    DuracaoMeses = 24,
                    MontanteInvestido = 20000,
                    TaxaJurosPadrao = 6.0
                };
                var result = await _controller.PutFundosInvestimento(fundo.Id, fundoDto);

                Assert.IsInstanceOf<NoContentResult>(result);
                var updatedFundo = await _context.FundosInvestimentos.FindAsync(fundo.Id);
                Assert.AreEqual("Fundo Atualizado", updatedFundo.Nome);
                Assert.AreEqual(20000, updatedFundo.MontanteInvestido);
            }

            [Test]
            public async Task DeleteFundosInvestimento_RemovesFundo()
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
                
                var fundo = new FundosInvestimento
                {
                    Id = Guid.NewGuid(),
                    AtivoId = ativo.Id,
                    Nome = "Fundo a Deletar",
                    DuracaoMeses = 12,
                    MontanteInvestido = 10000,
                    TaxaJurosPadrao = 5.5
                };
                _context.FundosInvestimentos.Add(fundo);
                _context.SaveChanges();

                var result = await _controller.DeleteFundosInvestimento(fundo.Id);
                Assert.IsInstanceOf<NoContentResult>(result);

                var deletedFundo = await _context.FundosInvestimentos.FindAsync(fundo.Id);
                Assert.IsNull(deletedFundo);
            }

            [Test]
            public async Task GetFundosInvestimentos_ReturnsList()
            {  var ativo = new AtivosFinanceiro
                {
                    Id = Guid.NewGuid(),
                    Tipo = "Ação",  
                    DataInicio = DateOnly.FromDateTime(DateTime.UtcNow),
                    TaxaPercentagem = 5.0
                };
                _context.AtivosFinanceiros.Add(ativo);
                await _context.SaveChangesAsync();
                
                _context.FundosInvestimentos.Add(new FundosInvestimento
                {
                    Id = Guid.NewGuid(),
                    AtivoId = ativo.Id,
                    Nome = "Fundo ABC",
                    DuracaoMeses = 12,
                    MontanteInvestido = 10000,
                    TaxaJurosPadrao = 5.5
                });
                _context.SaveChanges();

                var result = await _controller.GetFundosInvestimentos();

                Assert.IsInstanceOf<OkObjectResult>(result.Result);
                var okResult = result.Result as OkObjectResult;
                var fundos = okResult.Value as List<FundosInvestimentoDto>;
                Assert.IsNotNull(fundos);
                Assert.IsTrue(fundos.Count > 0);
            }

            [Test]
            public async Task GetFundosInvestimento_ReturnsNotFound_WhenIdIsInvalid()
            {
                var result = await _controller.GetFundosInvestimento(Guid.NewGuid());
                Assert.IsInstanceOf<NotFoundResult>(result.Result);
            }
        }
    }
}