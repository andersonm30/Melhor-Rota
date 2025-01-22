using System;
using System.Collections.Generic;
using MelhorRota.Domain.Interfaces;

namespace MelhorRota.Domain.Strategies
{
    public class DepthFirstSearchStrategy : IBuscaStrategy
    {
        private List<string> _melhorCaminho;
        private int _melhorCusto;

        public (List<string> Caminho, int Custo) BuscarMelhorRota(
            Dictionary<string, List<(string Destino, int Custo)>> grafo,
            string origem,
            string destino)
        {
            if (!grafo.ContainsKey(origem))
                return (new List<string>(), -1);

            _melhorCaminho = new List<string>();
            _melhorCusto = int.MaxValue;

            var caminhoAtual = new List<string>();
            var visitados = new HashSet<string>();

            DFS(grafo, origem, destino, 0, caminhoAtual, visitados);

            if (_melhorCusto == int.MaxValue)
                return (new List<string>(), -1);

            return (_melhorCaminho, _melhorCusto);
        }

        private void DFS(
            Dictionary<string, List<(string Destino, int Custo)>> grafo,
            string atual,
            string destino,
            int custoAcumulado,
            List<string> caminhoAtual,
            HashSet<string> visitados)
        {
            visitados.Add(atual);
            caminhoAtual.Add(atual);

            if (atual == destino)
            {
                if (custoAcumulado < _melhorCusto)
                {
                    _melhorCusto = custoAcumulado;
                    _melhorCaminho = new List<string>(caminhoAtual);
                }
            }
            else
            {
                if (grafo.ContainsKey(atual))
                {
                    foreach (var (proxDestino, proxCusto) in grafo[atual])
                    {
                        if (!visitados.Contains(proxDestino))
                        {
                            int novoCusto = custoAcumulado + proxCusto;
                            if (novoCusto < _melhorCusto)
                            {
                                DFS(grafo, proxDestino, destino, novoCusto, caminhoAtual, visitados);
                            }
                        }
                    }
                }
            }

            caminhoAtual.RemoveAt(caminhoAtual.Count - 1);
            visitados.Remove(atual);
        }
    }
}
