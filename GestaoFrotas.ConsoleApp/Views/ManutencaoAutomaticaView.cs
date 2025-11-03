using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class ManutencaoAutomaticaView
  {
    private readonly ManutencaoAutomaticaService _manutencaoService;
    private readonly VeiculoService _veiculoService;

    public ManutencaoAutomaticaView(ManutencaoAutomaticaService manutencaoService, VeiculoService veiculoService)
    {
      _manutencaoService = manutencaoService;
      _veiculoService = veiculoService;
    }

    public void ExibirMenuManutencao()
    {
      int pendentes = _manutencaoService.ListarPendentes().Count + _manutencaoService.ListarAgendadasParaConfirmacao().Count;

      while (true)
      {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" GERENCIAR MANUTENÇÕES AUTOMÁTICAS (RF05)"); 
        Console.WriteLine("=========================================");
        
        if (pendentes > 0)
        {
            Console.WriteLine($"[out] O sistema identificou {pendentes} manutenções automáticas pendentes."); 
        }

        Console.WriteLine("\n1 - Agendar Manutenção Automática (Pendentes)"); 
        Console.WriteLine("2 - Confirmar Agenda de Manutenção (Agendadas)"); 
        Console.WriteLine("3 - Cancelar Agenda de Manutenção (Ativas)"); 
        Console.WriteLine("4 - Pesquisar/Excluir Agenda (Histórico)"); 
        Console.WriteLine();
        Console.WriteLine("0 - Voltar ao Menu Principal"); 
        Console.WriteLine();
        Console.Write("Digite sua opção: "); 

        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            AgendarManutencao(); 
            break;
          case "2":
            ConfirmarManutencao(); 
            break;
          case "3":
            CancelarManutencao(); 
            break;
          case "4":
            PesquisarManutencao(); 
            break;
          case "0":
            return;
          default:
            Console.WriteLine("Opção inválida! Tente novamente.");
            PausarEVoltar();
            break;
        }
        pendentes = _manutencaoService.ListarPendentes().Count + _manutencaoService.ListarAgendadasParaConfirmacao().Count;
      }
    }
    
    private void AgendarManutencao()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" AGENDAR MANUTENÇÃO AUTOMÁTICA (RF05)"); 
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' a qualquer momento)"); 

      var pendentes = _manutencaoService.ListarPendentes();
      if (pendentes.Count == 0)
      {
          Console.WriteLine("\n[out] Nenhum veículo com critérios de manutenção atingidos no momento.");
          PausarEVoltar();
          return;
      }

      Console.WriteLine("\n[out] Veículos que necessitam de manutenção (Critério atingido):"); 
      
      foreach (var m in pendentes)
      {
          string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == m.veiculoId)?.placa ?? $"ID {m.veiculoId}";
          Console.WriteLine($" [ID: {m.id}] Placa: {placa} - Manutenção: {m.servico}");
      }
      
      int id = LerInteiro("\nDigite o ID da manutenção desejada: "); 
      if (id == 0) return;
      
      ManutencaoAutomatica manutencao = _manutencaoService.BuscarPorId(id);
      if(manutencao == null || manutencao.status != "Pendente")
      {
          Console.WriteLine("Erro: ID inválido ou manutenção não está pendente.");
          PausarEVoltar();
          return;
      }

      DateTime data = LerData("Digite a data desejada (dd/mm/aaaa): "); 
      
      Console.WriteLine("\n[out] Oficinas disponíveis (simulação):"); 
      Console.WriteLine(" 1. Oficina do Zé"); 
      Console.WriteLine(" 2. Mecânica Central"); 
      string oficina = LerInteiro("Digite a oficina desejada (1 ou 2): ") == 1 ? "Oficina do Zé" : "Mecânica Central"; 

      Console.WriteLine($"\n[out] Verificando disponibilidade da [{oficina}] para [{data:dd/MM/yyyy}]..."); 
      Console.WriteLine("[out] Agendamento disponível."); 

      if(ConfirmarOperacao("Confirmar agendamento? (S/N): ")) 
      {
          string resultado = _manutencaoService.Agendar(manutencao, data, oficina);
          Console.WriteLine("\n=========================================");
          Console.WriteLine(" AGENDAMENTO REALIZADO COM SUCESSO"); 
          Console.WriteLine($" {resultado}"); 
          Console.WriteLine("=========================================");
      }
      else
      {
          Console.WriteLine("\nAgendamento cancelado.");
      }
      
      PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar Manutenções'..."); 
    }
    
    private void ConfirmarManutencao()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CONFIRMAR AGENDA DE MANUTENÇÃO (RF05)"); 
      Console.WriteLine("=========================================");

      var agendadas = _manutencaoService.ListarAgendadasParaConfirmacao();
      if(agendadas.Count == 0)
      {
          Console.WriteLine("\n[out] Nenhuma agenda pendente de confirmação.");
          PausarEVoltar();
          return;
      }

      Console.WriteLine("\n[out] Agendas pendentes de sua confirmação:"); 
      foreach (var m in agendadas) 
      {
          string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == m.veiculoId)?.placa ?? $"ID {m.veiculoId}";
          Console.WriteLine($" [ID: {m.id}] Veículo: {placa}");
          Console.WriteLine($"   Serviço: {m.servico}");
          Console.WriteLine($"   Data: {m.data:dd/MM/yyyy}");
          Console.WriteLine($"   Oficina: {m.oficina}");
          Console.WriteLine("   --------------------");
      }
      
      int id = LerInteiro("Digite o ID da agenda para confirmar (ou 0 para voltar): "); 
      if (id == 0) return;

      ManutencaoAutomatica manutencao = _manutencaoService.BuscarPorId(id);
      if(manutencao == null || manutencao.status != "Agendada")
      {
          Console.WriteLine("Erro: ID inválido ou manutenção não está 'Agendada'.");
          PausarEVoltar();
          return;
      }
      
      if(ConfirmarOperacao($"\nConfirmar este agendamento [ID: {id}]? (S/N): ")) 
      {
          string resultado = _manutencaoService.Confirmar(manutencao);
          Console.WriteLine("\n=========================================");
          Console.WriteLine(" AGENDAMENTO CONFIRMADO COM SUCESSO"); 
          Console.WriteLine($" {resultado}"); 
          Console.WriteLine("=========================================");
      }
      else
      {
          Console.WriteLine("\nConfirmação cancelada.");
      }
      
      PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar Manutenções'..."); 
    }
    
    private void CancelarManutencao()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CANCELAR AGENDA DE MANUTENÇÃO (RF05)"); 
      Console.WriteLine("=========================================");
      
      var ativas = _manutencaoService.ListarAtivas();
      if(ativas.Count == 0)
      {
          Console.WriteLine("\n[out] Nenhuma agenda ativa (Agendada ou Confirmada) para cancelar.");
          PausarEVoltar();
          return;
      }

      Console.WriteLine("\n[out] Suas agendas ativas (Confirmadas ou Agendadas):"); 
      foreach (var m in ativas) 
      {
          string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == m.veiculoId)?.placa ?? $"ID {m.veiculoId}";
          Console.WriteLine($" [ID: {m.id}] Veículo: {placa} | Data: {m.data:dd/MM/yyyy} | Status: {m.status}");
      }
      
      int id = LerInteiro("\nDigite o ID da agenda que deseja CANCELAR (ou 0 para voltar): "); 
      if (id == 0) return;
      
      ManutencaoAutomatica manutencao = _manutencaoService.BuscarPorId(id);
      if(manutencao == null || (manutencao.status != "Agendada" && manutencao.status != "Confirmada"))
      {
          Console.WriteLine("Erro: ID inválido ou manutenção não está ativa.");
          PausarEVoltar();
          return;
      }

      if(ConfirmarOperacao($"Deseja realmente cancelar esta agenda [ID: {id}]? (S/N): ")) 
      {
          string resultado = _manutencaoService.Cancelar(manutencao);
          Console.WriteLine($"\n[out] {resultado}"); 
      }
      else
      {
          Console.WriteLine("\nOperação cancelada.");
      }

      PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar Manutenções'..."); 
    }
    
    private void PesquisarManutencao()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" PESQUISAR AGENDA AUTOMÁTICA (RF05)"); 
      Console.WriteLine("=========================================");
      
      Console.Write("Digite os critérios de busca (Placa, Data, Status ou Oficina): "); 
      string criterio = Console.ReadLine();
      
      var resultados = _manutencaoService.Pesquisar(criterio);
      Console.WriteLine($"\n[out] Exibindo resultados para \"{criterio}\"..."); 

      if (resultados.Count == 0)
      {
          Console.WriteLine("Nenhum registro encontrado.");
          PausarEVoltar();
          return;
      }
      
      foreach (var m in resultados) 
      {
          string placa = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == m.veiculoId)?.placa ?? $"ID {m.veiculoId}";
          Console.WriteLine($" [ID: {m.id}] Veículo: {placa} | Data: {m.data:dd/MM/yyyy} | Status: {m.status}");
      }
      
      Console.WriteLine("\nOpções:");
      Console.WriteLine("(1) Excluir Registro de Agenda (A4)");
      Console.WriteLine("(2) Voltar ao Menu");
      Console.Write("Digite sua opção: ");
      
      if (Console.ReadLine() == "1")
      {
          ProcessarExclusao(resultados); 
      }
    }
    
    private void ProcessarExclusao(List<ManutencaoAutomatica> resultados)
    {
        Console.WriteLine("\n=========================================");
        Console.WriteLine(" EXCLUIR REGISTRO DE AGENDA (A4)"); 
        Console.WriteLine("=========================================");
        
        int id = LerInteiro("Digite o ID da agenda que deseja EXCLUIR: "); 
        if (id == 0) return;
        
        ManutencaoAutomatica manutencao = resultados.FirstOrDefault(m => m.id == id);
        if(manutencao == null)
        {
            Console.WriteLine("Erro: ID não encontrado na lista de resultados.");
            PausarEVoltar();
            return;
        }

        Console.WriteLine($"\n[out] Dados da Agenda: [ID: {manutencao.id}] | Status: {manutencao.status}"); 
        
        if(ConfirmarOperacao("Deseja realmente excluir este REGISTRO de manutenção? (S/N): ")) 
        {
            string resultado = _manutencaoService.Excluir(manutencao);
            Console.WriteLine($"\n[out] {resultado}"); 
        }
        else
        {
            Console.WriteLine("\nExclusão cancelada.");
        }
        
        PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar Manutenções'..."); 
    }


    // --- Métodos Auxiliares (Helpers) ---
    private void PausarEVoltar(string mensagem = "Pressione qualquer tecla para voltar...")
    {
      Console.WriteLine();
      Console.WriteLine(mensagem);
      Console.ReadKey();
    }
    
    private int LerInteiro(string prompt)
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
          Console.WriteLine("Erro: Valor inválido. Digite um ID numérico (ou 0).");
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