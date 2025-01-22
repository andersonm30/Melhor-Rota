using Microsoft.VisualStudio.TestTools.UnitTesting;
using MelhorRota.Domain.Strategies;
using System.Collections.Generic;

namespace MelhorRota.Tests
{
    [TestClass]
    public class DepthFirstSearchStrategyTests
    {
        [TestMethod]
        public void Quando_OrigemInexistente_Entao_RetornaMenosUm()
        {
            var strategy = new DepthFirstSearchStrategy();
            var grafo = new Dictionary<string, List<(string Destino, int Custo)>>()
            {
                { "ABC", new List<(string, int)>{ ("XYZ", 10) } }
            };

            var (caminho, custo) = strategy.BuscarMelhorRota(grafo, "GRU", "CDG");

            Assert.AreEqual(-1, custo);
            Assert.AreEqual(0, caminho.Count);
        }

        [TestMethod]
        public void Quando_NaoHaCaminho_Entao_RetornaMenosUm()
        {
            // Arrange
            var strategy = new DepthFirstSearchStrategy();
            var grafo = new Dictionary<string, List<(string, int)>>()
            {
                { "GRU", new List<(string, int)>{ ("BRC", 10) } },
                { "BRC", new List<(string, int)>{ ("SCL", 5) } }
            };

            var (caminho, custo) = strategy.BuscarMelhorRota(grafo, "GRU", "CDG");

            Assert.AreEqual(-1, custo);
            Assert.AreEqual(0, caminho.Count);
        }
    }
}
