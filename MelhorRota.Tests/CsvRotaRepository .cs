using Microsoft.VisualStudio.TestTools.UnitTesting;
using MelhorRota.Domain.Interfaces;
using MelhorRota.Domain.Models;
using MelhorRota.Domain.Repositories;
using System;
using System.IO;
using System.Linq;

namespace MelhorRota.Tests
{
    [TestClass]
    public class CsvRotaRepositoryTests
    {
        private string _arquivoTeste = "repositorio_teste.csv";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_arquivoTeste))
            {
                File.Delete(_arquivoTeste);
            }
        }

        [TestMethod]
        public void Quando_ArquivoEstaVazio_ObterTodas_DeveRetornarVazio()
        {
            File.WriteAllText(_arquivoTeste, string.Empty);

            IRepository repo = new CsvRotaRepository(_arquivoTeste);
            var rotas = repo.ObterTodas();

            Assert.IsFalse(rotas.Any(), "Esperado que não haja rotas em arquivo vazio.");
        }

        [TestMethod]
        public void Quando_AdicionarRota_ElaDeveAparecerNoArquivo()
        {
            File.WriteAllText(_arquivoTeste, "");

            IRepository repo = new CsvRotaRepository(_arquivoTeste);

            repo.Adicionar(new Rota("AAA", "BBB", 100));

            var rotas = repo.ObterTodas().ToList();

            Assert.AreEqual(1, rotas.Count, "Esperado 1 rota após adicionar.");
            Assert.AreEqual("AAA", rotas[0].Origem);
            Assert.AreEqual("BBB", rotas[0].Destino);
            Assert.AreEqual(100, rotas[0].Custo);
        }

        [TestMethod]
        public void Quando_ArquivoTemVariasLinhas_ObterTodas_DeveRetornarTodas()
        {
            File.WriteAllLines(_arquivoTeste, new[]
            {
                "GRU,BRC,10",
                "BRC,SCL,5",
                "SCL,ORL,20"
            });

            IRepository repo = new CsvRotaRepository(_arquivoTeste);
            var rotas = repo.ObterTodas().OrderBy(r => r.Origem).ToList();

            Assert.AreEqual(3, rotas.Count);
            Assert.AreEqual("BRC", rotas[0].Origem);
            Assert.AreEqual("SCL", rotas[0].Destino);
            Assert.AreEqual(5, rotas[0].Custo);

            Assert.AreEqual("GRU", rotas[1].Origem);
            Assert.AreEqual("BRC", rotas[1].Destino);
            Assert.AreEqual(10, rotas[1].Custo);

            Assert.AreEqual("SCL", rotas[2].Origem);
            Assert.AreEqual("ORL", rotas[2].Destino);
            Assert.AreEqual(20, rotas[2].Custo);
        }
    }
}
