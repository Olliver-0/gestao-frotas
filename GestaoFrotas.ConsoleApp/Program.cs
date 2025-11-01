using System;
// Adiciona os 'using' para as pastas que criamos.
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;

// Define que o Program.cs está na "raiz" do seu projeto
namespace GestaoFrotas.ConsoleApp
{
  class Program
  {
    // Aqui é onde vocês vão instanciar as Views (Telas)
    // (Por enquanto, elas estão comentadas pois os arquivos ainda não existem)

    private static VeiculoService _veiculoService = new VeiculoService();
    private static VeiculoView _veiculoView = new VeiculoView(_veiculoService);

    private static AbastecimentoService _abastecimentoService = new AbastecimentoService();
    private static AbastecimentoView _abastecimentoView = new AbastecimentoView(_abastecimentoService, _veiculoService);
    // private static PecaView _pecaView = new PecaView();
    // private static OsView _osView = new OsView();
    // ...etc...

    static void Main(string[] args)
    {
      // O 'while(true)' cria o loop principal do menu.
      // O programa só fecha quando o usuário digita "0".
      while (true)
      {
        Console.Clear(); // Limpa a tela
        Console.WriteLine("=========================================");
        Console.WriteLine(" GESTÃO DE MANUTENÇÃO DE FROTAS");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("Menu Principal:");
        Console.WriteLine("1 - Gestão de Veículos");
        Console.WriteLine("2 - Gestão de Peças e Estoque");
        Console.WriteLine("3 - Gestão de Manutenção (Ordens de Serviço)");
        Console.WriteLine("4 - Registrar Abastecimento");
        Console.WriteLine("5 - Registrar Checklist Pré-Viagem");
        Console.WriteLine("6 - Relatórios"); // (RF10, RF12, RF13)
        Console.WriteLine();
        Console.WriteLine("0 - Sair do Sistema");
        Console.WriteLine();
        Console.Write("Digite sua opção: ");

        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            _veiculoView.ExibirMenuVeiculos();
            break;
          case "2":
            // _pecaView.ExibirMenuPecas(); // Pessoa 3 vai descomentar
            Console.WriteLine("Módulo de Peças em construção...");
            Console.ReadKey();
            break;
          case "3":
            // _osView.ExibirMenuOS(); // Pessoa 2 vai descomentar
            Console.WriteLine("Módulo de Manutenção em construção...");
            Console.ReadKey();
            break;
          case "4":
            _abastecimentoView.RegistrarAbastecimento();
            break;
          // ... (adicionar os outros casos) ...
          case "6":
            // ...
            Console.WriteLine("Módulo de Relatórios em construção...");
            Console.ReadKey();
            break;
          case "0":
            Console.WriteLine("Saindo do sistema. Até logo!");
            return; // Encerra o programa
          default:
            Console.WriteLine("Opção inválida! Tente novamente.");
            Console.ReadKey(); // Pausa para o usuário ler a msg
            break;
        }
      }
    }
  }
}
