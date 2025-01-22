using System.Collections.Generic;
using MelhorRota.Domain.Models;

namespace MelhorRota.Domain.Interfaces
{
    public interface IRepository
    {
        IEnumerable<Rota> ObterTodas();
        void Adicionar(Rota rota);
    }
}
