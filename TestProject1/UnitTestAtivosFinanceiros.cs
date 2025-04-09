using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;


namespace TestProject1
{
    public class UnitTests1
    {
        private AtivosFinanceirosController _controller;
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

            _controller = new AtivosFinanceirosController(_context);
        }

   
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAtivosFinanceiros_ReturnsList()
        {
            _context.AtivosFinanceiros.Add(new AtivosFinanceiro { Id = Guid.NewGuid(), Tipo = "Ação", DataInicio = DateOnly.FromDateTime(DateTime.UtcNow), TaxaPercentagem = 5 });
            _context.SaveChanges();

            var result = await _controller.GetAtivosFinanceiros();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var ativos = okResult.Value as List<AtivosFinanceiroDto>;
            Assert.IsNotNull(ativos);
            Assert.IsTrue(ativos.Count > 0);
        }

        [Test]
        public async Task GetAtivosFinanceiro_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetAtivosFinanceiro(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PostAtivosFinanceiro_CreatesNewAtivo()
        {
            var ativoDto = new AtivosFinanceiroDto { Tipo = "Fundo", DataInicio = DateOnly.FromDateTime(DateTime.UtcNow), TaxaPercentagem = 3 };
            var result = await _controller.PostAtivosFinanceiro(ativoDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }

        [Test]
        public async Task PutAtivosFinanceiro_UpdatesExistingAtivo()
        {
            var ativo = new AtivosFinanceiro { Id = Guid.NewGuid(), Tipo = "Ação", DataInicio = DateOnly.FromDateTime(DateTime.UtcNow), TaxaPercentagem = 5 };
            _context.AtivosFinanceiros.Add(ativo);
            _context.SaveChanges();

            var ativoDto = new AtivosFinanceiroDto { Id = ativo.Id, Tipo = "Novo Tipo", DataInicio = ativo.DataInicio, TaxaPercentagem = 6 };
            var result = await _controller.PutAtivosFinanceiro(ativo.Id, ativoDto);

            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedAtivo = await _context.AtivosFinanceiros.FindAsync(ativo.Id);
            Assert.AreEqual("Novo Tipo", updatedAtivo.Tipo);
        }

        [Test]
        public async Task DeleteAtivosFinanceiro_RemovesAtivo()
        {
            var ativo = new AtivosFinanceiro { Id = Guid.NewGuid(), Tipo = "Ação", DataInicio = DateOnly.FromDateTime(DateTime.UtcNow), TaxaPercentagem = 5 };
            _context.AtivosFinanceiros.Add(ativo);
            _context.SaveChanges();

            var result = await _controller.DeleteAtivosFinanceiro(ativo.Id);
            Assert.IsInstanceOf<NoContentResult>(result);

            var deletedAtivo = await _context.AtivosFinanceiros.FindAsync(ativo.Id);
            Assert.IsNull(deletedAtivo);
        }
    }
}
