using System;
using System.Collections.Generic;
using System.Globalization;
using GestaoFrotas.ConsoleApp.Models;
using GestaoFrotas.ConsoleApp.Services;

namespace GestaoFrotas.ConsoleApp.Views
{
  public class MotoristaView
  {
    private readonly MotoristaService _motoristaService;

    public MotoristaView(MotoristaService motoristaService)
    {
      _motoristaService = motoristaService;
    }

    public void ExibirMenuMotoristas()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" GESTÃO DE MOTORISTAS (RF02)"); 
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("1 - Cadastrar Motorista"); 
        Console.WriteLine("2 - Consultar Motorista"); 
        Console.WriteLine("3 - Editar Motorista"); 
        Console.WriteLine("4 - Excluir Motorista"); 
        Console.WriteLine("5 - Listar Todos");
        Console.WriteLine();
        Console.WriteLine("0 - Voltar ao Menu Principal"); 
        Console.WriteLine();
        Console.Write("Digite sua opção: "); 

        string opcao = Console.ReadLine();

        switch (opcao)
        {
          case "1":
            CadastrarMotorista();
            break;
          case "2":
            ConsultarMotorista();
            break;
          case "3":
            EditarMotorista();
            break;
          case "4":
            ExcluirMotorista();
            break;
          case "5":
            ListarTodosMotoristas();
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

    private void CadastrarMotorista()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CADASTRAR MOTORISTA (RF02)"); 
      Console.WriteLine("=========================================");
      Console.WriteLine("(Para cancelar, digite '0' a qualquer momento)"); 

      try
      {
        string cpf = LerString("Digite o CPF do motorista: "); 
        if (cpf == "0") return;
        
        Motorista novoMotorista = new Motorista();
        novoMotorista.cpf = cpf; 

        Console.WriteLine("\n--- Formulário de Cadastro ---");
        novoMotorista.nome = LerString("Nome Completo: "); 
        novoMotorista.cnh = LerString("CNH (número): "); 
        novoMotorista.categoriaCNH = LerString("Categoria CNH (ex: D): "); 
        
        // Adicionado para RN-002
        novoMotorista.validadeCNH = LerData("Validade CNH (dd/mm/aaaa): ");
        
        novoMotorista.telefone = LerString("Telefone (ex: (47) 99999-9999): "); 
        novoMotorista.endereco = LerString("Endereço (ex: Rua das Flores, 100 - Joinville/SC): "); 
        
        if (ConfirmarOperacao("Confirmar cadastro? (S/N): ")) 
        {
            string resultado = _motoristaService.Adicionar(novoMotorista);

            if (resultado.Contains("Erro:")) 
            {
              Console.WriteLine("\n=========================================");
              Console.WriteLine($" {resultado.ToUpper()}"); 
              Console.WriteLine("=========================================");
            }
            else 
            {
              Console.WriteLine("\n=========================================");
              Console.WriteLine(" MOTORISTA CADASTRADO COM SUCESSO"); 
              Console.WriteLine($" {novoMotorista.nome} (CPF {novoMotorista.cpf}) cadastrado com sucesso!"); 
              Console.WriteLine("=========================================");
            }
        }
        else
        {
            Console.WriteLine("\nCadastro cancelado.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }

      PausarEVoltar("Pressione qualquer tecla para voltar ao menu 'Gerenciar Motoristas'..."); 
    }

    private void ConsultarMotorista()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" CONSULTAR MOTORISTA (RF02)"); 
      Console.WriteLine("=========================================");

      string cpf = LerString("Digite o CPF do motorista: "); 
      Motorista motorista = _motoristaService.BuscarPorCpf(cpf);

      if (motorista != null)
      {
        Console.WriteLine("\n[out] Exibindo resultados..."); 
        ExibirDadosDoMotorista(motorista);

        Console.WriteLine("\n(1) Excluir Motorista");
        Console.WriteLine("(2) Voltar");
        Console.Write("Digite sua opção: ");
        string opcao = Console.ReadLine();
        if (opcao == "1")
        {
            ProcessarExclusao(motorista);
        }
      }
      else
      {
        Console.WriteLine("\nErro: Motorista não encontrado.");
      }

      PausarEVoltar();
    }

    private void EditarMotorista()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" EDITAR MOTORISTA (RF02)"); 
      Console.WriteLine("=========================================");

      string cpf = LerString("Digite o CPF do motorista: "); 
      Motorista motorista = _motoristaService.BuscarPorCpf(cpf);

      if (motorista == null)
      {
        Console.WriteLine("\nErro: Motorista não encontrado.");
        PausarEVoltar();
        return;
      }

      Console.WriteLine("\n[out] Dados encontrados."); 
      ExibirDadosDoMotorista(motorista);
      Console.WriteLine("------------------------");
      Console.WriteLine("Digite os novos valores (deixe em branco para manter o atual):");

      try
      {
        Console.Write($"Novo Telefone (Atual: {motorista.telefone}): ");
        string novoTelefone = Console.ReadLine();
        if (string.IsNullOrEmpty(novoTelefone)) novoTelefone = motorista.telefone;

        Console.Write($"Novo Endereço (Atual: {motorista.endereco}): ");
        string novoEndereco = Console.ReadLine();
        if (string.IsNullOrEmpty(novoEndereco)) novoEndereco = motorista.endereco;
        
        Console.Write($"Nova Categoria CNH (Atual: {motorista.categoriaCNH}): ");
        string novaCategoria = Console.ReadLine();
        if (string.IsNullOrEmpty(novaCategoria)) novaCategoria = motorista.categoriaCNH;

        if (ConfirmarOperacao("\nConfirmar alteração? (S/N): ")) 
        {
            string resultado = _motoristaService.Atualizar(cpf, novoTelefone, novoEndereco, novaCategoria);
            Console.WriteLine("\n=========================================");
            Console.WriteLine($" {resultado.ToUpper()}"); 
            Console.WriteLine("=========================================");
        }
        else 
        {
            Console.WriteLine("\n=========================================");
            Console.WriteLine(" ALTERAÇÃO CANCELADA"); 
            Console.WriteLine(" As modificações foram descartadas."); 
            Console.WriteLine("=========================================");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
      }

      PausarEVoltar("Pressione qualquer tecla para retornar ao menu..."); 
    }
    
    private void ExcluirMotorista()
    {
        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine(" EXCLUIR MOTORISTA (RF02)");
        Console.WriteLine("=========================================");

        string cpf = LerString("Digite o CPF do motorista: ");
        Motorista motorista = _motoristaService.BuscarPorCpf(cpf);

        if (motorista == null)
        {
            Console.WriteLine("\nErro: Motorista não encontrado.");
            PausarEVoltar();
            return;
        }
        
        ProcessarExclusao(motorista);
        PausarEVoltar("Pressione qualquer tecla para retornar ao menu 'Gerenciar Motoristas'...");
    }

    private void ProcessarExclusao(Motorista motorista)
    {
        Console.WriteLine($"\n--- Dados do Motorista ---");
        ExibirDadosDoMotorista(motorista);
        Console.WriteLine("--------------------------");
        
        if (ConfirmarOperacao($"Deseja realmente excluir o motorista {motorista.nome} (CPF {motorista.cpf})? (S/N): "))
        {
            string resultado = _motoristaService.Excluir(motorista.cpf);
            Console.WriteLine($"\n[out] {resultado}"); 
        }
        else
        {
            Console.WriteLine("\nOperação cancelada.");
        }
    }

    private void ListarTodosMotoristas()
    {
      Console.Clear();
      Console.WriteLine("=========================================");
      Console.WriteLine(" LISTA DE TODOS OS MOTORISTAS");
      Console.WriteLine("=========================================");

      List<Motorista> motoristas = _motoristaService.ListarTodos();

      if (motoristas.Count == 0)
      {
        Console.WriteLine("Nenhum motorista cadastrado.");
      }
      else
      {
        Console.WriteLine($"{"ID",-5} {"CPF",-16} {"Nome",-25} {"Status",-10}");
        Console.WriteLine(new string('-', 58));

        foreach (var m in motoristas)
        {
          Console.WriteLine($"{m.id,-5} {m.cpf,-16} {m.nome,-25} {m.status,-10}");
        }
      }

      PausarEVoltar();
    }

    private void ExibirDadosDoMotorista(Motorista m)
    {
      Console.WriteLine($"CPF: {m.cpf}"); 
      Console.WriteLine($"Nome: {m.nome}"); 
      Console.WriteLine($"CNH: {m.cnh} (Cat: {m.categoriaCNH})"); 
      Console.WriteLine($"Validade CNH: {m.validadeCNH:dd/MM/yyyy}"); 
      Console.WriteLine($"Telefone: {m.telefone}"); 
      Console.WriteLine($"Endereço: {m.endereco}");
      Console.WriteLine($"Status: {m.status}"); 
    }

    // --- Métodos Auxiliares (Helpers) ---

    private void PausarEVoltar(string mensagem = "Pressione qualquer tecla para voltar...")
    {
      Console.WriteLine();
      Console.WriteLine(mensagem);
      Console.ReadKey();
    }

    private string LerString(string prompt)
    {
        string entrada;
        do
        {
            Console.Write(prompt);
            entrada = Console.ReadLine();
            if (entrada == "0") return "0";
            if (string.IsNullOrEmpty(entrada))
            {
                Console.WriteLine("Erro: Campo obrigatório.");
            }
        } while (string.IsNullOrEmpty(entrada));
        return entrada;
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