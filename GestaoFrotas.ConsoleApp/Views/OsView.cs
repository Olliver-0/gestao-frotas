using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class OsView
  {
    private readonly OsService _osService;
    private readonly VeiculoService _veiculoService;
    private AddPecaOsView _addPecaOsView;

    public OsView(OsService osService, VeiculoService veiculoService, AddPecaOsView addPecaOsView)
    {
      _osService = osService;
      _veiculoService = veiculoService;
      _addPecaOsView= addPecaOsView;
    }

    public void ExibirMenuOS()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" GERENCIAR ORDEM DE SERVIÇO (RF06)");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("1 - Criar Ordem de Serviço");
        Console.WriteLine("2 - Consultar Ordem de Serviço");
        Console.WriteLine("3 - Adicionar peças na OS");
        Console.WriteLine("4 - Listar OS Abertas");
        Console.WriteLine("5 - Listar OS Finalizadas");
        Console.WriteLine();
        Console.WriteLine("0 - Voltar ao Menu Principal");
        Console.WriteLine();
        Console.Write("Digite sua opção: ");

        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            CriarOs();
            break;
          case "2":
            ConsultarOs();
            break;

          case "3":
            _addPecaOsView.ViewExibirMenuAddPecaOS();
            break;
          case "4":
            ListarOsPorStatus("Aberta");
            break;
          case "5":
            ListarOsPorStatus("Finalizada");
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

    private void CriarOs()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CRIAR ORDEM DE SERVIÇO (RF06)");
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' a qualquer momento)");

      try
      {
        Console.Write("Digite a Placa do Veículo: ");
        string placa = Console.ReadLine();
        if (placa == "0") return;

        // Adicionado para RN-002
        Console.Write("Digite o CPF do Motorista: ");
        string motoristaCpf = Console.ReadLine();
        if (motoristaCpf == "0") return;

        Console.Write("Digite o Tipo de Serviço (ex: Correção, Preventiva): ");
        string tipo = Console.ReadLine();
        if (tipo == "0") return;

        Console.Write("Digite a Oficina (Nome): ");
        string oficina = Console.ReadLine();
        if (oficina == "0") return;

        Console.Write("Digite a Descrição do Problema/Serviço: ");
        string descricao = Console.ReadLine();
        if (descricao == "0") return;

        OrdemDeServico novaOs = new OrdemDeServico
        {
          tipo = tipo,
          oficina = oficina,
          descricao = descricao
        };

        if (ConfirmarOperacao("\nConfirmar criação da OS? (S/N): "))
        {
          string resultado = _osService.Adicionar(novaOs, placa, motoristaCpf);

          if (resultado.Contains("Erro:"))
          {
            Console.WriteLine("\n=========================================");
            Console.WriteLine($" ERRO - {resultado.ToUpper()}");
            Console.WriteLine("=========================================");
          }
          else
          {
            Console.WriteLine("\n=========================================");
            Console.WriteLine(" ORDEM DE SERVIÇO CRIADA");
            Console.WriteLine($" {resultado}");
            Console.WriteLine("=========================================");
          }
        }
        else
        {
          Console.WriteLine("\nCriação cancelada.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }

      PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar OS'...");
    }

    private void ConsultarOs()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CONSULTAR ORDEM DE SERVIÇO (RF06)");
      Console.WriteLine("=========================================");

      int id = LerInteiro("Digite o ID da OS: ");
      if (id == 0) return;

      OrdemDeServico os = _osService.BuscarPorId(id);

      if (os != null)
      {
        Console.WriteLine("\n[out] Exibindo resultados...");
        ExibirDadosDaOs(os);

        // Opções após consultar (Fluxo A1)
        Console.WriteLine("\n(1) Finalizar esta OS");
        Console.WriteLine("(2) Excluir esta OS");
        Console.WriteLine("(3) Voltar");
        Console.Write("Digite sua opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            // Inicia fluxo RF07 + RF06
            FinalizarOs(os);
            break;
          case "2":
            // Inicia fluxo A2
            ProcessarExclusao(os);
            break;
          case "3":
            return;
        }
      }
      else
      {
        Console.WriteLine("\nErro: Ordem de Serviço não encontrada.");
      }

      PausarEVoltar();
    }

    private void ListarOsPorStatus(string status)
    {
      Console.Clear();
      Console.WriteLine($"=========================================");
      Console.WriteLine($" LISTA DE OS COM STATUS: {status.ToUpper()}");
      Console.WriteLine($"=========================================");

      List<OrdemDeServico> lista = _osService.ListarPorStatus(status);

      if (lista.Count == 0)
      {
        Console.WriteLine($"Nenhuma OS encontrada com status '{status}'.");
      }
      else
      {
        Console.WriteLine($"{"ID",-5} {"Placa",-10} {"Oficina",-20} {"Serviço",-25}");
        Console.WriteLine(new string('-', 62));

        foreach (var os in lista)
        {
          string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == os.veiculoId)?.placa ?? "N/A";
          Console.WriteLine($"{os.id,-5} {placa,-10} {os.oficina,-20} {os.tipo,-25}");
        }
      }
      PausarEVoltar();
    }

    // Fluxo A3 (RF06) + RF07
    private void FinalizarOs(OrdemDeServico os)
    {
      if (os.status != "Aberta")
      {
        Console.WriteLine($"\nErro: Esta OS (ID {os.id}) já está com status '{os.status}' e não pode ser finalizada.");
        return;
      }

      try
      {
        // --- ETAPA 1: FLUXO RF07 (Obrigatório) ---
        bool validacaoOk = ExecutarValidacaoRF07(os);

        if (!validacaoOk)
        {
          // Fluxo de Exceção A1 (RF07)
          Console.WriteLine("\n=========================================");
          Console.WriteLine(" FINALIZAÇÃO CANCELADA.");
          Console.WriteLine(" Os documentos não foram validados.");
          Console.WriteLine("=========================================");
          return;
        }

        // --- ETAPA 2: FLUXO RF06 (Continuação) ---
        Console.WriteLine("\n=========================================");
        Console.WriteLine($" FINALIZAR OS [ID {os.id}]");
        Console.WriteLine(" ETAPA 2 de 2: DADOS DE FINALIZAÇÃO (RF06)");
        Console.WriteLine("=========================================");
        Console.WriteLine("[out] Documentos validados. Insira os dados de fechamento.");

        double custoFinal = LerDouble("Digite o Custo Final (ex: 850,00): ");
        DateTime dataConclusao = LerData("Digite a Data de Conclusão (dd/mm/aaaa): ");

        // Adicionado para RN-005
        double hodometroSaida = LerDouble($"Digite o Hodômetro de Saída (Km) (Mínimo: {os.hodometroEntrada}): ");

        Console.Write("Digite alguma observação de fechamento (opcional): ");
        string observacoes = Console.ReadLine();

        if (ConfirmarOperacao("\nConfirmar finalização? (S/N): "))
        {
          string resultado = _osService.Finalizar(os, custoFinal, dataConclusao, observacoes, hodometroSaida);

          if (resultado.Contains("Erro:"))
          {
            Console.WriteLine("\n=========================================");
            Console.WriteLine($" ERRO - {resultado.ToUpper()}");
            Console.WriteLine("=========================================");
          }
          else
          {
            Console.WriteLine("\n=========================================");
            Console.WriteLine(" ORDEM DE SERVIÇO FINALIZADA");
            Console.WriteLine($" {resultado}");
            Console.WriteLine("=========================================");
          }
        }
        else
        {
          Console.WriteLine("\nFinalização cancelada.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }
    }

    private bool ExecutarValidacaoRF07(OrdemDeServico os)
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine($" FINALIZAR OS [ID {os.id}]");
      Console.WriteLine(" ETAPA 1 de 2: ANEXAR E VALIDAR DOCUMENTOS (RF07)");
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' a qualquer momento)");

      while (true)
      {
        Console.Write("Digite o caminho do arquivo (ex: C:\\NFs\\NF_123.pdf) (ou 'N' para continuar): ");
        string path = Console.ReadLine();

        if (path.Equals("0", StringComparison.OrdinalIgnoreCase))
          return false; // Cancela fluxo

        if (path.Equals("N", StringComparison.OrdinalIgnoreCase))
          break;

        Console.WriteLine($"[out] Arquivo [{path.Split('\\').Last()}] anexado com sucesso.");
      }

      // Validação
      if (ConfirmarOperacao("\n[] Documentos anexados foram conferidos e validados? (S/N): "))
      {
        os.validarDocumentos();
        return true; // Sucesso
      }

      return false;
    }

    // Fluxo A2
    private void ProcessarExclusao(OrdemDeServico os)
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine($" EXCLUIR ORDEM DE SERVIÇO [ID {os.id}]");
      Console.WriteLine("=========================================");
      Console.WriteLine($"Placa: {_veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == os.veiculoId)?.placa ?? "N/A"}");
      Console.WriteLine($"Serviço: {os.tipo}");
      Console.WriteLine($"Status: {os.status}");
      Console.WriteLine("\n(Esta ação não pode ser desfeita)");

      if (ConfirmarOperacao("\nDeseja realmente excluir esta Ordem de Serviço? (S/N): "))
      {
        string resultado = _osService.Excluir(os.id);
        Console.WriteLine($"\n[out] {resultado}");
      }
      else
      {
        Console.WriteLine("\nOperação cancelada.");
      }
    }

    public void ExibirDadosDaOs(OrdemDeServico os)
    {
      string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == os.veiculoId)?.placa ?? "N/A";

      Console.WriteLine($"ID: {os.id}");
      Console.WriteLine($"Placa: {placa}");
      Console.WriteLine($"Serviço: {os.tipo}");
      Console.WriteLine($"Oficina: {os.oficina}");
      Console.WriteLine($"Status: {os.status}");
      Console.WriteLine($"Descrição: {os.descricao}");
      Console.WriteLine($"Data Abertura: {os.dataAbertura:dd/MM/yyyy}");
      Console.WriteLine($"Hodômetro Entrada: {os.hodometroEntrada} Km");


      if (os.status == "Finalizada")
      {
        Console.WriteLine("--- DADOS DE FECHAMENTO ---");
        Console.WriteLine($"Custo Final: {os.custoFinal:C2}");
        Console.WriteLine($"Data Conclusão: {os.dataFechamento:dd/MM/yyyy}");
        Console.WriteLine($"Hodômetro Saída: {os.hodometroSaida} Km");
        Console.WriteLine($"Observações: {os.observacoesFechamento}");
        Console.WriteLine($"Documentos Validados (RF07): {os.documentosValidados}");
      }
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
          Console.WriteLine("Erro: Valor inválido. Digite um número positivo (ex: 850,00).");
        }
      }
    }

    public int LerInteiro(string prompt)
    {
      int valor;
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine();
        if (int.TryParse(entrada, out valor) && valor >= 0)
        {
          return valor;
        }
        else
        {
          Console.WriteLine("Erro: Valor inválido. Digite um ID numérico.");
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

    private bool ConfirmarOperacao(string prompt)
    {
      while (true)
      {
        Console.Write(prompt);
        string entrada = Console.ReadLine().ToUpper();
        if (entrada == "S") return true;
        if (entrada == "N") return false;
        Console.WriteLine("Erro: Digite apenas S (Sim) ou N (Não).");
      }
    }
  }
}
