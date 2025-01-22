using System;
using System.IO;
using System.Linq;
using MelhorRota.Domain.Interfaces;
using MelhorRota.Domain.Repositories;
using MelhorRota.Domain.Services;
using MelhorRota.Domain.Strategies;

namespace MelhorRota.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string caminhoArquivo = "rotas.csv";
                        
            IRepository repository = new CsvRotaRepository(caminhoArquivo);
            IBuscaStrategy strategy = new DepthFirstSearchStrategy();
            IRotaService rotaService = new RotaService(repository, strategy);

            if (ArquivoEstaVazioOuNaoExiste(caminhoArquivo))
            {
                InserirRotasIniciais(rotaService);
            }

            var menuService = new MenuService(rotaService);
            menuService.ExecutarMenuPrincipal();
        }

        private static bool ArquivoEstaVazioOuNaoExiste(string caminho)
        {
            if (!File.Exists(caminho))
                return true; 

            var linhas = File.ReadAllLines(caminho);
            return !linhas.Any();
        }

        private static void InserirRotasIniciais(IRotaService rotaService)
        {
            Console.WriteLine(">>> Inicializando rotas iniciais...\n");
           
            rotaService.AdicionarRota("GRU", "BRC", 10);
            rotaService.AdicionarRota("BRC", "SCL", 5);
            rotaService.AdicionarRota("GRU", "CDG", 75);
            rotaService.AdicionarRota("GRU", "SCL", 20);
            rotaService.AdicionarRota("GRU", "ORL", 56);
            rotaService.AdicionarRota("ORL", "CDG", 5);
            rotaService.AdicionarRota("SCL", "ORL", 20);

            Console.WriteLine(">>> Rotas iniciais cadastradas com sucesso!\n");
        }
    }
}
