namespace MelhorRota.Domain.Models
{
     public class Rota
    {
        public string Origem { get; }
        public string Destino { get; }
        public int Custo { get; }

        public Rota(string origem, string destino, int custo)
        {
            Origem = origem;
            Destino = destino;
            Custo = custo;
        }

        public override string ToString() => $"{Origem}-{Destino} ({Custo})";
    }
}
