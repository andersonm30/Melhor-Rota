using System.Collections.Generic;

namespace MelhorRota.Domain.Interfaces
{

    public interface IBuscaStrategy
    {
        (List<string> Caminho, int Custo) BuscarMelhorRota(
            Dictionary<string, List<(string Destino, int Custo)>> grafo,
            string origem,
            string destino
        );
    }
}
