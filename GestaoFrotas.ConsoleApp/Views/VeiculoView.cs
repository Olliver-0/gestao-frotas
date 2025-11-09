using System;
using System.Collections.Generic;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class VeiculoView
  {
    private readonly VeiculoService _veiculoService;
    private readonly MotoristaService _motoristaService;
    public VeiculoView(VeiculoService veiculoService, MotoristaService motoristaService)
    {
      _veiculoService = veiculoService;
      _motoristaService = motoristaService;
    }

    public void ExibirMenuVeiculos()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" GESTÃO DE VEÍCULOS (RF01)");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("Selecione uma opção:");
        Console.WriteLine("1 - Cadastrar Novo Veículo");
        Console.WriteLine("2 - Consultar Veículo");
        Console.WriteLine("3 - Editar Veículo");
        Console.WriteLine("4 - Inativar Veículo");
        Console.WriteLine("5 - Listar Todos os Veículos");
        Console.WriteLine();
        Console.WriteLine("0 - Voltar ao Menu Principal");
        Console.WriteLine();
        Console.Write("Digite sua opção: ");

        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            CadastrarNovoVeiculo();
            break;
          case "2":
            ConsultarVeiculo();
            break;
          case "3":
            EditarVeiculo();
            break;
          case "4":
            InativarVeiculo();
            break;
          case "5":
            ListarTodosVeiculos();
            break;
          case "0":
            return;
          default:
            Console.WriteLine("Opção inválida! Tente novamente.");
            PausarEVoltar();
            break;
        }
      }
    }

    private void CadastrarNovoVeiculo()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CADASTRAR NOVO VEÍCULO (RF01)");
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, deixe a Placa em branco e tecle Enter)");

      try
      {
        Veiculo novoVeiculo = new Veiculo();

        Console.Write("Placa (ex: BRA2E19): ");
        novoVeiculo.placa = Console.ReadLine();
        if (string.IsNullOrEmpty(novoVeiculo.placa))
          return;

        novoVeiculo.hodometroInicial = LerDouble("Hodômetro Inicial (Km): ");
        novoVeiculo.hodometroAtual = novoVeiculo.hodometroInicial;

        novoVeiculo.dataAquisicao = LerData("Data de Aquisição (dd/mm/aaaa): ");

        novoVeiculo.capacidadeCarga = LerDouble("Capacidade de Carga (Kg): ");

        novoVeiculo.vencimentoLicenciamento = LerData("Data de Venc. do Licenciamento (dd/mm/aaaa): ");

        string resultado = _veiculoService.Adicionar(novoVeiculo);

        if (resultado.Contains("Erro:"))
        {
          Console.WriteLine("\n=========================================");
          Console.WriteLine($" ERRO - {resultado.ToUpper()}");
          Console.WriteLine("=========================================");
        }
        else
        {
          Console.WriteLine("\n[SUCESSO]");
          Console.WriteLine(resultado);
          Console.WriteLine($"ID de Frota gerado (RN-001): {novoVeiculo.idFrota}");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }

      PausarEVoltar();
    }

    private void ConsultarVeiculo()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CONSULTAR VEÍCULO (RF01)");
      Console.WriteLine("=========================================");

      Veiculo veiculo = BuscarVeiculoPorPlacaOuId("consultar");

      if (veiculo != null)
      {
        ExibirDadosDoVeiculo(veiculo);
      }

      PausarEVoltar();
    }

    private void EditarVeiculo()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" EDITAR VEÍCULO (RF01)");
      Console.WriteLine("=========================================");

      Veiculo veiculoOriginal = BuscarVeiculoPorPlacaOuId("editar");

      if (veiculoOriginal == null)
      {
        PausarEVoltar();
        return;
      }

      Console.WriteLine("\n--- DADOS ATUAIS ---");
      ExibirDadosDoVeiculo(veiculoOriginal);
      Console.WriteLine("--------------------");
      Console.WriteLine("Digite os novos valores (deixe em branco para manter o atual):");

      try
      {
        Veiculo veiculoAtualizado = new Veiculo
        {
          idFrota = veiculoOriginal.idFrota,
          placa = veiculoOriginal.placa,
          hodometroInicial = veiculoOriginal.hodometroInicial,
          dataAquisicao = veiculoOriginal.dataAquisicao,

          status = veiculoOriginal.status,
          hodometroAtual = veiculoOriginal.hodometroAtual,
          vencimentoLicenciamento = veiculoOriginal.vencimentoLicenciamento,
          capacidadeCarga = veiculoOriginal.capacidadeCarga
        };

        Console.Write($"Status (Atual: {veiculoOriginal.status}): ");
        string novoStatus = Console.ReadLine();
        if (!string.IsNullOrEmpty(novoStatus))
          veiculoAtualizado.status = novoStatus;

          veiculoAtualizado.vencimentoLicenciamento = LerData(
      $"Venc. Licenciamento (Atual: {veiculoOriginal.vencimentoLicenciamento:dd/MM/yyyy}): ",
      true
        );
        // Se o usuário pulou (e o resultado é 01/01/0001), mantém o original
        if (veiculoAtualizado.vencimentoLicenciamento == DateTime.MinValue)
        {
          veiculoAtualizado.vencimentoLicenciamento = veiculoOriginal.vencimentoLicenciamento;
        }


        veiculoAtualizado.hodometroAtual = LerDouble(
      $"Hodômetro Atual (Atual: {veiculoOriginal.hodometroAtual} Km): ",
      true
        );

        if (veiculoAtualizado.hodometroAtual == 0)
        {
          veiculoAtualizado.hodometroAtual = veiculoOriginal.hodometroAtual;
        }

        veiculoAtualizado.capacidadeCarga = LerDouble(
      $"Capacidade de Carga (Atual: {veiculoOriginal.capacidadeCarga} Kg): ",
      true
        );

        if (veiculoAtualizado.capacidadeCarga == 0)
        {
          veiculoAtualizado.capacidadeCarga = veiculoOriginal.capacidadeCarga;
        }

        string resultado = _veiculoService.Atualizar(veiculoAtualizado);

        if (resultado.Contains("Erro:"))
        {
          Console.WriteLine("\n=========================================");
          Console.WriteLine($" ERRO - {resultado.ToUpper()}");
          Console.WriteLine("=========================================");
        }
        else
        {
          Console.WriteLine("\n[SUCESSO]");
          Console.WriteLine(resultado);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }

      PausarEVoltar();
    }

    private void InativarVeiculo()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" INATIVAR VEÍCULO (RF01)");
      Console.WriteLine("=========================================");

      Veiculo veiculo = BuscarVeiculoPorPlacaOuId("inativar");

      if (veiculo == null)
      {
        PausarEVoltar();
        return;
      }

      Console.WriteLine("\n--- DADOS DO VEÍCULO ---");
      ExibirDadosDoVeiculo(veiculo);
      Console.WriteLine("------------------------");

      if (veiculo.status == "Inativo")
      {
        Console.WriteLine("Este veículo já está inativo.");
        PausarEVoltar();
        return;
      }

      Console.Write($"\nDeseja realmente inativar o veículo {veiculo.placa}? (S/N): ");
      string confirmacao = Console.ReadLine().ToUpper();

      // 3. Chamar o Serviço (ou cancelar)
      if (confirmacao == "S")
      {
        string resultado = _veiculoService.Inativar(veiculo);
        Console.WriteLine("\n[SUCESSO]");
        Console.WriteLine(resultado); // Tela 1.4.3a
      }
      else
      {
        Console.WriteLine("\nOperação cancelada."); // Tela 1.4.3b
      }

      PausarEVoltar();
    }

    private void ListarTodosVeiculos()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" LISTA DE TODOS OS VEÍCULOS");
      Console.WriteLine("=========================================");

      List<Veiculo> veiculos = _veiculoService.ListarTodos();

      if (veiculos.Count == 0)
      {
        Console.WriteLine("Nenhum veículo cadastrado.");
      }
      else
      {
       Console.WriteLine($"{"ID Frota",-10} {"Placa",-10} {"Status",-15} {"Hodômetro",-12} {"Licenc.",-8}");
        Console.WriteLine(new string('-', 60));

        foreach (var v in veiculos)
        {
          string statusLicenciamento = v.isLicenciamentoVencido() ? "VENCIDO" : "OK";
          Console.WriteLine($"{v.idFrota,-10} {v.placa,-10} {v.status,-15} {v.hodometroAtual,-12} Km {statusLicenciamento,-8}");
        }
      }

      PausarEVoltar();
    }

    private void PausarEVoltar(string mensagem = "Pressione qualquer tecla para voltar...")
    {
      Console.WriteLine();
      Console.WriteLine(mensagem);
      Console.ReadKey();
    }

    private Veiculo BuscarVeiculoPorPlacaOuId(string acao)
    {
      Console.Write($"Digite a Placa ou o ID da Frota do veículo que deseja {acao}: ");
      string busca = Console.ReadLine();

      Veiculo veiculo = _veiculoService.BuscarPorPlacaOuId(busca);

      if (veiculo == null)
      {
        Console.WriteLine("\n=========================================");
        Console.WriteLine(" ERRO - VEÍCULO NÃO ENCONTRADO");
        Console.WriteLine("=========================================");
        return null;
      }

      return veiculo;
    }

    private void ExibirDadosDoVeiculo(Veiculo v)
    {
      Console.WriteLine($"ID Frota: {v.idFrota}");
      Console.WriteLine($"Placa: {v.placa}");
      Console.WriteLine($"Status: {v.status}");
      Console.WriteLine($"Hodômetro Atual: {v.hodometroAtual} Km");
      Console.WriteLine($"Hodômetro Inicial: {v.hodometroInicial} Km");
      Console.WriteLine($"Capacidade Carga: {v.capacidadeCarga} Kg");
      Console.WriteLine($"Data Aquisição: {v.dataAquisicao:dd/MM/yyyy}");
      // Adiciona verificação do status de licenciamento
     Console.WriteLine($"Venc. Licenciamento: {v.vencimentoLicenciamento:dd/MM/yyyy} ({(v.isLicenciamentoVencido() ? "VENCIDO" : "OK")})");
    }

    private double LerDouble(string prompt, bool permitirEmBranco = false)
    {
      double valor;
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine();

        if (permitirEmBranco && string.IsNullOrEmpty(entrada))
        {
          return 0;
        }

        if (double.TryParse(entrada, out valor) && valor >= 0)
        {
          return valor;
        }
        else
        {
          Console.WriteLine("Erro: Valor inválido. Digite um número positivo (ex: 150 ou 50.5).");
        }
      }
    }

    private DateTime LerData(string prompt, bool permitirEmBranco = false)
    {
      DateTime data;
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine();

        if (permitirEmBranco && string.IsNullOrEmpty(entrada))
        {
          return DateTime.MinValue;
        }

        if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
        {
          return data;
        }
        else
        {
          Console.WriteLine("Erro: Formato de data inválido. Use dd/mm/aaaa.");
        }
      }
    }
  }
}
