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

        Console.WriteLine("\n--- DADOS DO VEÍCULO ---");
        Console.WriteLine($"Veículo: {veiculo.placa} (ID Frota: {veiculo.idFrota})");
        Console.WriteLine($"Hodômetro Atual: {veiculo.hodometroAtual} Km");
        Console.WriteLine("------------------------");

        Abastecimento novoAbastecimento = new Abastecimento();
        novoAbastecimento.veiculoId = veiculo.idFrota;

        novoAbastecimento.data = LerData("Data do Abastecimento (dd/mm/aaaa): ");
        novoAbastecimento.litrosAbastecidos = LerDouble("Litros Abastecidos (ex: 50.5): ");
        novoAbastecimento.valorTotal = LerDouble("Valor Total (R$) (ex: 300.25): ");

        novoAbastecimento.hodometro = LerDouble($"Quilometragem atual do veículo (RF09): ");

        string resultado = _abastecimentoService.Adicionar(novoAbastecimento, veiculo);

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

    private Veiculo BuscarVeiculoPorPlacaOuId(string acao)
    {
      Console.Write($"\nDigite a Placa ou o ID da Frota do veículo que deseja {acao}: ");
      string busca = Console.ReadLine();

      if (busca == "0") return null;

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
          Console.WriteLine("Erro: Formato de data inválido. Use dd/mm/aaaa.");
        }
      }
    }
  }
}
