using System;
using System.Collections.Generic;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class MotoristaService
  {
    private static readonly List<Motorista> _motoristas = new List<Motorista>();
    private static int _proximoId = 1;

    public string Adicionar(Motorista motorista)
    {
      // RN: O sistema deve impedir o cadastro de motoristas com CPF duplicado.
      if (_motoristas.Any(m => m.login == motorista.login))
      {
        return "Erro: CPF já cadastrado. Não é possível duplicar o motorista."; 
      }

      motorista.id = _proximoId++;
      motorista.status = "Ativo"; 
      _motoristas.Add(motorista);

      return "Motorista cadastrado com sucesso.";
    }

    public Motorista BuscarPorCpf(string cpf)
    {
      return _motoristas.FirstOrDefault(m => m.login == cpf && m.status != "Excluído");
    }

    public List<Motorista> ListarTodos()
    {
      return _motoristas.Where(m => m.status != "Excluído").ToList();
    }

    public string Atualizar(string cpfBusca, string novoTelefone, string novoEndereco, string novaCategoriaCNH)
    {
      Motorista motorista = BuscarPorCpf(cpfBusca);
      if (motorista == null)
      {
        return "Erro: Motorista não encontrado.";
      }

      motorista.telefone = novoTelefone;
      motorista.endereco = novoEndereco;
      motorista.categoriaCNH = novaCategoriaCNH;

      return "Alteração realizada com sucesso.";
    }

    public string Excluir(string cpf)
    {
      Motorista motorista = BuscarPorCpf(cpf);
      if (motorista == null)
      {
        return "Erro: Motorista não encontrado.";
      }
      
      motorista.status = "Excluído"; 

      return "Motorista excluído com sucesso.";
    }
  }
}   