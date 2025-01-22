using System.Collections.Generic;
using MelhorRota.Domain.Models;

namespace MelhorRota.Domain.Interfaces
{
    public interface IRotaService
    {
        (List<string> Caminho, int Custo) ObterMelhorRota(string origem, string destino);

        void AdicionarRota(string origem, string destino, int custo);

        IEnumerable<Rota> ObterTodasAsRotas();
    }
}
