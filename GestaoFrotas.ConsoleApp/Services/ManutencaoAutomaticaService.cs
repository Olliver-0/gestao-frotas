using System;
using System.Collections.Generic;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class ManutencaoAutomaticaService
  {
    private static readonly List<ManutencaoAutomatica> _manutencoes = new List<ManutencaoAutomatica>();
    private readonly VeiculoService _veiculoService;

    public ManutencaoAutomaticaService(VeiculoService veiculoService)
    {
      _veiculoService = veiculoService;
      
      if (!_manutencoes.Any())
      {
        var veiculo1 = _veiculoService.BuscarPorPlacaOuId("ABC-1234");
        var veiculo2 = _veiculoService.BuscarPorPlacaOuId("XYZ-7890");
        var veiculo3 = _veiculoService.BuscarPorPlacaOuId("DEF-5678");

        int v1_id = veiculo1?.idFrota ?? 101;
        int v2_id = veiculo2?.idFrota ?? 102;
        int v3_id = veiculo3?.idFrota ?? 103;

        _manutencoes.Add(new ManutencaoAutomatica(101, v1_id, "Troca de Óleo (KM Atingido)", "Pendente"));
        _manutencoes.Add(new ManutencaoAutomatica(102, v2_id, "Revisão Periódica (Tempo Atingido)", "Pendente"));
        _manutencoes.Add(new ManutencaoAutomatica(105, v3_id, "Revisão de Freios", "Agendada", new DateTime(2025, 11, 22), "Freios Bons"));
      }
    }
    public List<ManutencaoAutomatica> ListarPendentes()
    {
      return _manutencoes.Where(m => m.status == "Pendente").ToList();
    }

    public ManutencaoAutomatica BuscarPorId(int id)
    {
        return _manutencoes.FirstOrDefault(m => m.id == id);
    }
    
    public string Agendar(ManutencaoAutomatica manutencao, DateTime data, string oficina)
    {
      manutencao.data = data;
      manutencao.oficina = oficina;
      manutencao.status = "Agendada"; 
      return $"Manutenção automática ({manutencao.servico}) agendada com sucesso para {data:dd/MM/yyyy} na {oficina}."; 
    }
    public List<ManutencaoAutomatica> ListarAgendadasParaConfirmacao()
    {
        return _manutencoes.Where(m => m.status == "Agendada").ToList();
    }
    
    public string Confirmar(ManutencaoAutomatica manutencao)
    {
        manutencao.status = "Confirmada"; 
        return $"A agenda [ID: {manutencao.id}] foi confirmada."; 
    }
    public List<ManutencaoAutomatica> ListarAtivas()
    {
        return _manutencoes.Where(m => m.status == "Agendada" || m.status == "Confirmada").ToList();
    }

    public string Cancelar(ManutencaoAutomatica manutencao)
    {
        manutencao.status = "Cancelada"; 
        return "Agendamento cancelado com sucesso."; 
    }
    public List<ManutencaoAutomatica> Pesquisar(string criterio)
    {
        return _manutencoes.Where(m => 
            (m.oficina != null && m.oficina.Contains(criterio, StringComparison.OrdinalIgnoreCase)) ||
            (m.status != null && m.status.Contains(criterio, StringComparison.OrdinalIgnoreCase)) ||
            (m.servico != null && m.servico.Contains(criterio, StringComparison.OrdinalIgnoreCase))
        ).ToList();
    }
    public string Excluir(ManutencaoAutomatica manutencao)
    {
        if(manutencao.status == "Agendada" || manutencao.status == "Confirmada")
        {
            return "Erro: Não é possível excluir uma manutenção ativa. Use 'Cancelar'.";
        }
        
        _manutencoes.Remove(manutencao);
        return "Agenda removida com sucesso."; 
    }
  }
}