using MelhorRota.Domain.Interfaces;
using MelhorRota.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace MelhorRota.Domain.Services
{
    public class RotaService : IRotaService
    {
        private readonly IRepository _repository;
        private readonly IBuscaStrategy _buscaStrategy;

        public RotaService(IRepository repository, IBuscaStrategy buscaStrategy)
        {
            _repository = repository;
            _buscaStrategy = buscaStrategy;
        }

        public (List<string> Caminho, int Custo) ObterMelhorRota(string origem, string destino)
        {
            var grafo = MontarGrafo();
            return _buscaStrategy.BuscarMelhorRota(grafo, origem, destino);
        }

        public void AdicionarRota(string origem, string destino, int custo)
        {
            var rota = new Rota(origem, destino, custo);
            _repository.Adicionar(rota);
        }

        public IEnumerable<Rota> ObterTodasAsRotas()
        {
            return _repository.ObterTodas();
        }

        private Dictionary<string, List<(string Destino, int Custo)>> MontarGrafo()
        {
            var rotas = _repository.ObterTodas();
            var grafo = new Dictionary<string, List<(string, int)>>();

            foreach (var r in rotas)
            {
                if (!grafo.ContainsKey(r.Origem))
                {
                    grafo[r.Origem] = new List<(string, int)>();
                }
                grafo[r.Origem].Add((r.Destino, r.Custo));
            }

            return grafo;
        }
    }
}
