using Microsoft.VisualStudio.TestTools.UnitTesting;
using MelhorRota.Domain.Interfaces;
using MelhorRota.Domain.Repositories;
using MelhorRota.Domain.Services;
using MelhorRota.Domain.Strategies;
using System;
using System.IO;
using System.Linq;

namespace MelhorRota.Tests
{
    [TestClass]
    public class RotaServiceTests
    {
        private string _arquivoTeste = "rotas_service_teste.csv";
        private IRotaService _rotaService;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllLines(_arquivoTeste, new[]
            {
                "GRU,BRC,10",
                "BRC,SCL,5",
                "GRU,CDG,75",
                "GRU,SCL,20",
                "GRU,ORL,56",
                "ORL,CDG,5",
                "SCL,ORL,20"
            });

            IRepository repository = new CsvRotaRepository(_arquivoTeste);
            IBuscaStrategy strategy = new DepthFirstSearchStrategy();
            _rotaService = new RotaService(repository, strategy);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_arquivoTeste))
            {
                File.Delete(_arquivoTeste);
            }
        }

        [TestMethod]
        public void Quando_ConsultarMelhorRota_GRU_CDG_DeveRetornarCusto40()
        {
            var (caminho, custo) = _rotaService.ObterMelhorRota("GRU", "CDG");
            Assert.AreEqual(40, custo);
            Assert.IsTrue(caminho.Any());

        }

        [TestMethod]
        public void Quando_AdicionarNovaRota_ConsultaDeveEncontrarEssaRota()
        {
            _rotaService.AdicionarRota("GRU", "XYZ", 10);

            var (caminho, custo) = _rotaService.ObterMelhorRota("GRU", "XYZ");
            Assert.AreEqual(10, custo);
            Assert.AreEqual(2, caminho.Count);
        }

        [TestMethod]
        public void Quando_NaoExisteRota_ObterMelhorRota_RetornaMenosUm()
        {
            var (caminho, custo) = _rotaService.ObterMelhorRota("AAA", "ZZZ");
            Assert.AreEqual(-1, custo);
            Assert.AreEqual(0, caminho.Count);
        }
    }
}
