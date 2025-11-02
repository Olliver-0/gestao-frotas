using System;
using System.Collections.Generic;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class ChecklistService
  {
    private static readonly List<ChecklistPreViagem> _checklists = new List<ChecklistPreViagem>();

    public string Adicionar(ChecklistPreViagem novoChecklist, Veiculo veiculo)
    {
      if (novoChecklist.hodometro < veiculo.hodometroAtual)
      {
        return $"Erro: Hodômetro inválido. O valor ({novoChecklist.hodometro}) deve ser maior ou igual ao atual ({veiculo.hodometroAtual}).";
      }

      veiculo.atualizarHodometro(novoChecklist.hodometro);

      _checklists.Add(novoChecklist);

      return "Checklist registrado com sucesso.";
    }
  }
}
