using System;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class ChecklistView
  {

    private readonly ChecklistService _checklistService;
    private readonly VeiculoService _veiculoService;


    public ChecklistView(ChecklistService checklistService, VeiculoService veiculoService)
    {
      _checklistService = checklistService;
      _veiculoService = veiculoService;
    }



    public void ExecutarChecklist()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" EXECUTAR CHECKLIST PRÉ-VIAGEM (RF11)");
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' na busca)");

      try
      {

        Veiculo veiculo = BuscarVeiculoPorPlacaOuId("executar o checklist");

        if (veiculo == null)
        {
          PausarEVoltar();
          return;
        }

        if (veiculo.status == "Inativo")
        {
          Console.WriteLine("\nErro: Não é possível executar checklist para um veículo Inativo.");
          PausarEVoltar();
          return;
        }



        int motoristaIdLogado = 1;

        Console.WriteLine("\n--- DADOS DO VEÍCULO ---");
        Console.WriteLine($"Veículo: {veiculo.placa} (ID Frota: {veiculo.idFrota})");
        Console.WriteLine($"Hodômetro Atual: {veiculo.hodometroAtual} Km");
        Console.WriteLine("------------------------");



        double hodometroInformado = LerDouble($"Para iniciar, informe a Quilometragem atual (RF09/RN-007): ");


        Console.WriteLine("\nResponda aos itens de segurança (S/N):");
        bool respostaPneus = LerSimNao("1. Pneus estão calibrados e em boas condições? (S/N): ");
        bool respostaOleo = LerSimNao("2. Nível do Óleo do motor está OK? (S/N): ");



        ChecklistPreViagem novoChecklist = new ChecklistPreViagem();
        novoChecklist.veiculoId = veiculo.idFrota;
        novoChecklist.motoristaId = motoristaIdLogado;


        novoChecklist.preencherRepostas(hodometroInformado, respostaPneus, respostaOleo);


        string resultado = _checklistService.Adicionar(novoChecklist, veiculo);


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
          Console.WriteLine("Boa viagem!");
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

        if (double.TryParse(entrada, out valor) && valor >= 0)
        {
          return valor;
        }
        else
        {
          Console.WriteLine("Erro: Valor inválido. Digite um número positivo (ex: 5050).");
        }
      }
    }

    private bool LerSimNao(string prompt)
    {
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine().ToUpper();

        if (entrada == "S")
        {
          return true;
        }
        else if (entrada == "N")
        {
          return false;
        }
        else
        {
          Console.WriteLine("Erro: Resposta inválida. Digite apenas 'S' (Sim) ou 'N' (Não).");
        }
      }
    }
  }
}
