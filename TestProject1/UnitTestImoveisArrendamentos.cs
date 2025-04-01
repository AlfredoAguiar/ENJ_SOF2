using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;

namespace TestProject1
{
    [TestFixture]
    public class ImoveisArrendamentoControllerTests
    {
        private ImoveisArrendamentoController _controller;
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

            _controller = new ImoveisArrendamentoController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task PostImoveisArrendamento_CreatesNewImovel()
        {
            
            var localizacao = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Imóvel"
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var imovelDto = new ImoveisArrendamentoDto
            {
                AtivoId = ativo.Id,
                Designacao = "Imóvel Teste",
                LocalizacaoId = localizacao.Id,
                ValorPropriedade = 500000,
                ValorRenda = 2000,
                TaxaCondominio = 200,
                DespesasAnuais = 2400,
                PercentagemPropriedade = 100
            };

            // Act
            var result = await _controller.PostImoveisArrendamento(imovelDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
        }

        [Test]
        public async Task PutImoveisArrendamento_UpdatesExistingImovel()
        {
   
            var localizacao = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Imóvel"
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var imovel = new ImoveisArrendamento
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Designacao = "Imóvel Inicial",
                LocalizacaoId = localizacao.Id,
                ValorPropriedade = 500000,
                ValorRenda = 2000,
                TaxaCondominio = 200,
                DespesasAnuais = 2400,
                PercentagemPropriedade = 100
            };
            _context.ImoveisArrendamentos.Add(imovel);
            await _context.SaveChangesAsync();

            var imovelDto = new ImoveisArrendamentoDto
            {
                Id = imovel.Id,
                Designacao = "Imóvel Atualizado",
                ValorPropriedade = 600000,
                ValorRenda = 2500,
                TaxaCondominio = 250,
                DespesasAnuais = 3000,
                PercentagemPropriedade = 100
            };

            // Act
            var result = await _controller.PutImoveisArrendamento(imovel.Id, imovelDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedImovel = await _context.ImoveisArrendamentos.FindAsync(imovel.Id);
            Assert.AreEqual("Imóvel Atualizado", updatedImovel.Designacao);
            Assert.AreEqual(600000, updatedImovel.ValorPropriedade);
        }

        [Test]
        public async Task DeleteImoveisArrendamento_RemovesImovel()
        {
            var localizacao = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Imóvel"
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            var imovel = new ImoveisArrendamento
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Designacao = "Imóvel a Deletar",
                LocalizacaoId = localizacao.Id,
                ValorPropriedade = 500000,
                ValorRenda = 2000,
                TaxaCondominio = 200,
                DespesasAnuais = 2400,
                PercentagemPropriedade = 100
            };
            _context.ImoveisArrendamentos.Add(imovel);
            await _context.SaveChangesAsync();
            
            var result = await _controller.DeleteImoveisArrendamento(imovel.Id);
            
            Assert.IsInstanceOf<NoContentResult>(result);

            var deletedImovel = await _context.ImoveisArrendamentos.FindAsync(imovel.Id);
            Assert.IsNull(deletedImovel);
        }

        [Test]
        public async Task GetImoveisArrendamento_ReturnsList()
        {
            var localizacao = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = "Rua A",
                Cidade = "Cidade A",
                Pais = "País A",
                CodigoPostal = "12345"
            };
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            var ativo = new AtivosFinanceiro
            {
                Id = Guid.NewGuid(),
                Tipo = "Imóvel"
            };
            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            _context.ImoveisArrendamentos.Add(new ImoveisArrendamento
            {
                Id = Guid.NewGuid(),
                AtivoId = ativo.Id,
                Designacao = "Imóvel Teste",
                LocalizacaoId = localizacao.Id,
                ValorPropriedade = 500000,
                ValorRenda = 2000,
                TaxaCondominio = 200,
                DespesasAnuais = 2400,
                PercentagemPropriedade = 100
            });
            await _context.SaveChangesAsync();
            
            var result = await _controller.GetImoveisArrendamento();
            
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var imoveis = okResult.Value as List<ImoveisArrendamentoDto>;
            Assert.IsNotNull(imoveis);
            Assert.IsTrue(imoveis.Count > 0);
        }

        [Test]
        public async Task GetImoveisArrendamento_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetImoveisArrendamento(Guid.NewGuid());

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}
