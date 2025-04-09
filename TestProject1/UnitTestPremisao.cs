using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication5.DTO;

namespace TestProject1
{
    public class PermissoControllerTests
    {
        private PermissoController _controller;
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

            _controller = new PermissoController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        #region Get Tests

        [Test]
        public async Task GetPermissoes_ReturnsList()
        {
            _context.Permissoes.Add(new Permisso { Id = Guid.NewGuid(), PodeCriarUtilizadores = true, PodeEditarUtilizadores = false, PodeVerRegistos = true, PodeAlterarPermissoes = false });
            _context.SaveChanges();
            
            var result = await _controller.GetPermissoes();
            
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var permissoes = okResult.Value as List<PermissoDto>;
            Assert.IsNotNull(permissoes);
            Assert.IsTrue(permissoes.Count > 0);
        }

        [Test]
        public async Task GetPermisso_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.GetPermisso(Guid.NewGuid());
            
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        #endregion

        #region Post Tests

        [Test]
        public async Task PostPermisso_CreatesNewPermisso()
        {
            var permisssoDto = new PermissoDto
            {
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = true,
                PodeVerRegistos = false,
                PodeAlterarPermissoes = false
            };
            
            var result = await _controller.CreatePermisso(permisssoDto);
            
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var createdPermissoDto = createdResult.Value as PermissoDto;
            Assert.IsNotNull(createdPermissoDto);
        }

        #endregion

        #region Put Tests

        [Test]
        public async Task PutPermisso_UpdatesExistingPermisso()
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

            var permisssoDto = new PermissoDto
            {
                Id = permissao.Id,
                PodeCriarUtilizadores = false,
                PodeEditarUtilizadores = true,
                PodeVerRegistos = true,
                PodeAlterarPermissoes = true
            };
            
            var result = await _controller.UpdatePermisso(permissao.Id, permisssoDto);

            Assert.IsInstanceOf<NoContentResult>(result);

            var updatedPermisso = await _context.Permissoes.FindAsync(permissao.Id);
            Assert.AreEqual(permisssoDto.PodeCriarUtilizadores, updatedPermisso.PodeCriarUtilizadores);
            Assert.AreEqual(permisssoDto.PodeEditarUtilizadores, updatedPermisso.PodeEditarUtilizadores);
        }

        [Test]
        public async Task PutPermisso_ReturnsNotFound_WhenIdIsInvalid()
        {
 
            var permisssoDto = new PermissoDto
            {
                PodeCriarUtilizadores = true,
                PodeEditarUtilizadores = true,
                PodeVerRegistos = false,
                PodeAlterarPermissoes = false
            };
            
            var result = await _controller.UpdatePermisso(Guid.NewGuid(), permisssoDto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        #endregion

        #region Delete Tests

        [Test]
        public async Task DeletePermisso_RemovesPermisso()
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

            var result = await _controller.DeletePermisso(permissao.Id);
            
            Assert.IsInstanceOf<NoContentResult>(result);

            var deletedPermisso = await _context.Permissoes.FindAsync(permissao.Id);
            Assert.IsNull(deletedPermisso);
        }

        [Test]
        public async Task DeletePermisso_ReturnsNotFound_WhenIdIsInvalid()
        {
            var result = await _controller.DeletePermisso(Guid.NewGuid());
            
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        #endregion
    }
}
