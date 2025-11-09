using System;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class AbastecimentoView
  {
    private readonly AbastecimentoService _abastecimentoService;
    private readonly VeiculoService _veiculoService;

    public AbastecimentoView(AbastecimentoService abastecimentoService, VeiculoService veiculoService)
    {
      _abastecimentoService = abastecimentoService;
      _veiculoService = veiculoService;
    }
  public void ExibirMenuAbastecimentos()
  {
      while (true)
    {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" MENU DE ABASTECIMENTOS (RF10)");
        Console.WriteLine("=========================================");
        Console.WriteLine("1 - Registrar Abastecimento");
        Console.WriteLine("0 - Voltar ao Menu Principal");
        Console.Write("\nEscolha uma opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                RegistrarAbastecimento();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Opção inválida!");
                Console.ReadKey();
                break;
        }
    }
  }

    public void RegistrarAbastecimento()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" REGISTRAR ABASTECIMENTO (RF10)");
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' na busca)");

      try
      {
        Veiculo veiculo = BuscarVeiculoPorPlacaOuId("abastecer");

        if (veiculo == null)
        {
          PausarEVoltar();
          return;
        }

        if (veiculo.status == "Inativo")
        {
          Console.WriteLine("\nErro: Não é possível registrar abastecimento para um veículo Inativo.");
          PausarEVoltar();
          return;
        }

        Console.WriteLine($"\n--- VEÍCULO SELECIONADO: {veiculo.placa} (ID Frota: {veiculo.idFrota}) ---");
        Console.WriteLine($" Último Hodômetro Registrado: {veiculo.hodometroAtual:N2} Km");

        DateTime data = LerData("Data do Abastecimento (dd/MM/yyyy): ");
        double litrosAbastecidos = LerDouble("Litros Abastecidos: ");
        double valorTotal = LerDouble("Valor Total da Nota: R$ ");
        double hodometro;
        while (true)
        {
          hodometro = LerDouble($"Hodômetro Atual (deve ser >= {veiculo.hodometroAtual:N2} Km): ");
          if (hodometro >= veiculo.hodometroAtual)
          {
            break;
          }
          Console.WriteLine($"Erro: O hodômetro deve ser maior ou igual ao atual ({veiculo.hodometroAtual:N2} Km).");
        }

        var novoAbastecimento = new Abastecimento
        {
          data = data,
          litrosAbastecidos = litrosAbastecidos,
          valorTotal = valorTotal,
          hodometro = hodometro,
          veiculoId = veiculo.idFrota
        };

        string resultado = _abastecimentoService.Adicionar(novoAbastecimento, veiculo);

        Console.WriteLine("\n--- RESULTADO DA OPERAÇÃO ---");
        Console.WriteLine(resultado);

      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nERRO INESPERADO: {ex.Message}");
      }
      PausarEVoltar();
    }

    private Veiculo BuscarVeiculoPorPlacaOuId(string acao)
    {
      string busca;
      Veiculo veiculo = null;

      while (veiculo == null)
      {
        Console.Write($"\nDigite a Placa ou ID da Frota do veículo que deseja {acao} (ou '0' para cancelar): ");
        busca = Console.ReadLine().ToUpper().Trim();

        if (busca == "0") return null;
        veiculo = _veiculoService.BuscarPorPlacaOuId(busca);

        if (veiculo == null)
        {
          Console.WriteLine("\n=========================================");
          Console.WriteLine(" ERRO - VEÍCULO NÃO ENCONTRADO. TENTE NOVAMENTE.");
          Console.WriteLine("=========================================");
        }
      }

      return veiculo;
    }

    private void PausarEVoltar(string mensagem = "Pressione qualquer tecla para voltar...")
    {
      Console.WriteLine();
      Console.WriteLine(mensagem);
      Console.ReadKey();
    }

    private double LerDouble(string prompt)
    {
      double valor;
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine();
        if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.CurrentCulture, out valor) && valor >= 0)
        {
          return valor;
        }
        else
        {
          Console.WriteLine("Erro: Valor inválido. Digite um número positivo (ex: 1500 ou 50,5).");
        }
      }
    }

    private DateTime LerData(string prompt)
    {
      DateTime data;
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine();

        if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
        {
          return data;
        }
        else
        {
          Console.WriteLine("Erro: Data inválida. Digite no formato DD/MM/AAAA.");
        }
      }
    }
  }
}
