using MelhorRota.Domain.Interfaces;
using MelhorRota.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MelhorRota.Domain.Repositories
{
    public class CsvRotaRepository : IRepository
    {
        private readonly string _caminhoArquivo;

        public CsvRotaRepository(string caminhoArquivo)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
                throw new ArgumentException("Caminho do arquivo inválido.", nameof(caminhoArquivo));

            _caminhoArquivo = caminhoArquivo;

            if (!File.Exists(_caminhoArquivo))
            {
                using (File.Create(_caminhoArquivo)) { }
            }
        }

        public IEnumerable<Rota> ObterTodas()
        {
            var rotas = new List<Rota>();
            var linhas = File.ReadAllLines(_caminhoArquivo);

            foreach (var linha in linhas)
            {
                var partes = linha.Split(',');
                if (partes.Length != 3) continue;

                string origem = partes[0].Trim();
                string destino = partes[1].Trim();
                if (int.TryParse(partes[2], out int custo))
                {
                    rotas.Add(new Rota(origem, destino, custo));
                }
            }

            return rotas;
        }

        public void Adicionar(Rota rota)
        {
            using (var sw = new StreamWriter(_caminhoArquivo, true))
            {
                sw.WriteLine($"{rota.Origem},{rota.Destino},{rota.Custo}");
            }
        }
    }
}
