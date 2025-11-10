using System;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;
using GestaoFrotas.ConsoleApp.Views;

namespace GestaoFrotas.ConsoleApp
{
  class Program
  {
    private static VeiculoService _veiculoService = new VeiculoService();
    private static AbastecimentoService _abastecimentoService = new AbastecimentoService();
    private static ChecklistService _checklistService = new ChecklistService();
    private static MotoristaService _motoristaService = new MotoristaService();
    private static OsService _osService = new OsService(_veiculoService, _motoristaService);
    private static PecaService _pecaService = new PecaService();
    
    private static AddPecaOsService _addPecaOsService = new AddPecaOsService(_osService, _veiculoService, _motoristaService, _pecaService);
    
    private static ManutencaoAutomaticaService _manutencaoAutomaticaService = new ManutencaoAutomaticaService(_veiculoService);
    private static AddPecaOsView _addPecaOsView = new AddPecaOsView(_osService, _veiculoService, _motoristaService, _pecaService, _addPecaOsService);    // --- Views ---


    private static VeiculoView _veiculoView = new VeiculoView(_veiculoService);
    private static AbastecimentoView _abastecimentoView = new AbastecimentoView(_abastecimentoService, _veiculoService);
    private static ChecklistView _checklistView = new ChecklistView(_checklistService, _veiculoService);
    private static PecaView _pecaView = new PecaView(_pecaService);
    private static OsView _osView = new OsView(_osService, _veiculoService, _addPecaOsView);
    private static ManutencaoAutomaticaView _manutencaoAutomaticaView = new ManutencaoAutomaticaView(_manutencaoAutomaticaService, _veiculoService);
    private static MotoristaView _motoristaView = new MotoristaView(_motoristaService, _osService);


    static void Main(string[] args)
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" GESTÃO DE MANUTENÇÃO DE FROTAS");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("Menu Principal:");
        Console.WriteLine("\n--- MÓDULOS COORDENADOR ---");
        Console.WriteLine("1 - Gestão de Veículos (RF01)");
        Console.WriteLine("2 - Gestão de Peças e Estoque");
        Console.WriteLine("3 - Gestão de Manutenção (OS) (RF06/07)");
        Console.WriteLine("7 - Gestão de Motoristas (RF02)");
        Console.WriteLine("6 - Relatórios (RN-010)");

        Console.WriteLine("\n--- MÓDULOS MOTORISTA ---");
        Console.WriteLine("4 - Registrar Abastecimento (RN-006)");
        Console.WriteLine("5 - Registrar Checklist Pré-Viagem (RN-007)");
        Console.WriteLine("8 - Manutenções Automáticas (RF05)");

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
            _pecaView.ViewExibirMenuPeca();
            break;
          case "3":
            _osView.ExibirMenuOS();
            break;
          case "4":
            _abastecimentoView.RegistrarAbastecimento();
            break;
          case "5":
            _checklistView.ExecutarChecklist();
            break;
          case "6":
            Console.WriteLine("Módulo de Relatórios em construção...");
            Console.ReadKey();
            break;
          case "7":
            _motoristaView.ExibirMenuMotoristas();
            break;
          case "8":
            _manutencaoAutomaticaView.ExibirMenuManutencao();
            break;
          case "0":
            Console.WriteLine("Saindo do sistema. Até logo!");
            return;
          default:
            Console.WriteLine("Opção inválida! Tente novamente.");
            Console.ReadKey();
            break;
        }
      }
    }
  }
}