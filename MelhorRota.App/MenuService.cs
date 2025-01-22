using System;
using System.Collections.Generic;
using System.Linq;
using MelhorRota.Domain.Interfaces;

namespace MelhorRota.App
{
    public class MenuService
    {
        private readonly IRotaService _rotaService;

        public MenuService(IRotaService rotaService)
        {
            _rotaService = rotaService;
        }

        public void ExecutarMenuPrincipal()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("=== Bem-vindo ao Melhor Rota ===");
                Console.WriteLine("1) Consultar melhor rota");
                Console.WriteLine("2) Registrar nova rota (ex.: GRU,SCL,30)");
                Console.WriteLine("3) Sair");

                Console.Write("\nOpção: ");
                var opcao = Console.ReadLine()?.Trim();

                switch (opcao)
                {
                    case "1":
                        ConsultarMelhorRota();
                        break;
                    case "2":
                        RegistrarNovaRota();
                        break;
                    case "3":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida.\n");
                        break;
                }
            }

            Console.WriteLine("Programa encerrado. Obrigado!");
        }

        private void ConsultarMelhorRota()
        {
            var todasRotas = _rotaService.ObterTodasAsRotas().ToList();
            if (!todasRotas.Any())
            {
                Console.WriteLine("\nNão há rotas cadastradas no momento.\n");
                return;
            }

            var todosAeroportos = todasRotas
                .SelectMany(r => new[] { r.Origem, r.Destino })
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            if (todosAeroportos.Count < 2)
            {
                Console.WriteLine("\nNão há aeroportos suficientes para montar uma rota.\n");
                return;
            }

            bool continuarConsulta = true;
            while (continuarConsulta)
            {
                Console.WriteLine("\n=== CONSULTA DE MELHOR ROTA ===");

                Console.WriteLine("\nSelecione o aeroporto de ORIGEM (digite o número):");
                for (int i = 0; i < todosAeroportos.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {todosAeroportos[i]}");
                }

                Console.Write($"\nEscolha (1 a {todosAeroportos.Count}): ");
                if (!int.TryParse(Console.ReadLine(), out int indiceOrigem)
                    || indiceOrigem < 1
                    || indiceOrigem > todosAeroportos.Count)
                {
                    Console.WriteLine("\nOpção inválida para origem.\n");
                    continue;
                }

                var origem = todosAeroportos[indiceOrigem - 1];

                Console.WriteLine("\nSelecione o aeroporto de DESTINO (digite o número):");
                for (int i = 0; i < todosAeroportos.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {todosAeroportos[i]}");
                }

                Console.Write($"\nEscolha (1 a {todosAeroportos.Count}): ");
                if (!int.TryParse(Console.ReadLine(), out int indiceDestino)
                    || indiceDestino < 1
                    || indiceDestino > todosAeroportos.Count)
                {
                    Console.WriteLine("\nOpção inválida para destino.\n");
                    continue;
                }

                var destino = todosAeroportos[indiceDestino - 1];

                if (origem == destino)
                {
                    Console.WriteLine("\nOrigem e destino não podem ser iguais.\n");
                    continue;
                }

                var (caminho, custo) = _rotaService.ObterMelhorRota(origem, destino);
                if (custo < 0 || caminho.Count == 0)
                {
                    Console.WriteLine($"\nNenhuma rota encontrada de {origem} para {destino}.\n");
                }
                else
                {
                    Console.WriteLine($"\nMelhor rota: {string.Join(" - ", caminho)} ao custo de ${custo}\n");
                }

                Console.WriteLine("O que deseja agora?");
                Console.WriteLine("1) Nova consulta");
                Console.WriteLine("2) Voltar ao menu anterior");
                Console.Write("\nOpção: ");
                var subOpcao = Console.ReadLine()?.Trim();

                switch (subOpcao)
                {
                    case "1":
                        break; 
                    case "2":
                        continuarConsulta = false; 
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida. Voltando ao menu principal.\n");
                        continuarConsulta = false;
                        break;
                }
            }
        }

        private void RegistrarNovaRota()
        {
            Console.WriteLine("\n=== REGISTRAR NOVA ROTA ===");
            Console.WriteLine("Informe a rota no formato ORIGEM,DESTINO,CUSTO (ex.: GRU,SCL,30)");
            Console.Write("Digite: ");

            var input = Console.ReadLine()?.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(input) || !input.Contains(','))
            {
                Console.WriteLine("\nFormato inválido.\n");
                return;
            }

            var partes = input.Split(',');
            if (partes.Length != 3)
            {
                Console.WriteLine("\nFormato inválido.\n");
                return;
            }

            var origem = partes[0];
            var destino = partes[1];
            if (!int.TryParse(partes[2], out int custo))
            {
                Console.WriteLine("\nCusto inválido.\n");
                return;
            }

            _rotaService.AdicionarRota(origem, destino, custo);
            Console.WriteLine("\nRota adicionada com sucesso!\n");
        }
    }
}
