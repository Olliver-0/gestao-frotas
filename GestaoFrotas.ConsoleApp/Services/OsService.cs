using System;
using System.Collections.Generic;
using System.Linq;
using GestaoFrotas.ConsoleApp.Models;

namespace GestaoFrotas.ConsoleApp.Services
{
  public class OsService
  {
    private static readonly List<OrdemDeServico> _ordensDeServico = new List<OrdemDeServico>();
    private static int _proximoId = 123;

    private readonly VeiculoService _veiculoService;
    private readonly MotoristaService _motoristaService;

    public OsService(VeiculoService veiculoService, MotoristaService motoristaService)
    {
      _veiculoService = veiculoService;
      _motoristaService = motoristaService;
    }

    public List<OrdemDeServico> ListarPorStatus(string status)
    {
      return _ordensDeServico;
    }
    public List<OrdemDeServico> ListarPorMotoristaId(int motoristaId)
    {
    return _ordensDeServico
        .Where(os => os.motoristaId == motoristaId && os.status != "Excluída")
        .ToList();
    }


    public string Adicionar(OrdemDeServico os, string placa, string motoristaCpf)
    {
      Veiculo veiculo = _veiculoService.BuscarPorPlacaOuId(placa);
      if (veiculo == null)
      {
        return "Erro: Veículo não localizado.";
      }

      Motorista motorista = _motoristaService.BuscarPorCpf(motoristaCpf);
      if (motorista == null)
      {
        return "Erro: Motorista não localizado.";
      }

      if (veiculo.isLicenciamentoVencido())
      {
        return $"Erro: Criação de OS bloqueada. O licenciamento do veículo {veiculo.placa} está vencido (ID: {veiculo.idFrota}).";
      }

      os.id = _proximoId++;
      os.dataAbertura = DateTime.Now;
      os.status = "Aberta";
      os.veiculoId = veiculo.idFrota;
      os.motoristaId = motorista.id;

      _ordensDeServico.Add(os);

      veiculo.definirStatus("Em Manutenção");

      return $"Sucesso: OS [ID: {os.id}] criada para o veículo {veiculo.placa}.";
    }

    public OrdemDeServico BuscarPorId(int id)
    {
      return _ordensDeServico.FirstOrDefault(os => os.id == id && os.status != "Excluída");
    }

    public string Excluir(int id)
    {
      OrdemDeServico os = BuscarPorId(id);
      if (os == null)
      {
        return "Erro: OS não encontrada.";
      }

      if (os.status == "Finalizada")
      {
        return "Erro: Não é possível excluir uma OS já finalizada.";
      }

      os.status = "Excluída";
      return $"OS [ID {id}] excluída com sucesso.";
    }

    public string Finalizar(OrdemDeServico os, double custoFinal, DateTime dataConclusao, string observacoes, double hodometroSaida)
    {
      if (!os.documentosValidados)
      {
        return "Erro: Os documentos precisam ser validados (Etapa 1/2) antes de finalizar a OS.";
      }

      if (os.status != "Aberta")
      {
        return $"Erro: OS com status '{os.status}' não pode ser finalizada.";
      }

      if (hodometroSaida < os.hodometroEntrada)
      {
        return $"Erro: Hodômetro de saída ({hodometroSaida} Km) não pode ser menor que o hodômetro de entrada ({os.hodometroEntrada} Km).";
      }

      os.fecharOS(custoFinal, dataConclusao, observacoes, hodometroSaida);

      Veiculo veiculo = _veiculoService.ListarTodos().FirstOrDefault(v => v.idFrota == os.veiculoId);
      if (veiculo != null)
      {
        veiculo.atualizarHodometro(hodometroSaida);
        veiculo.definirStatus("Disponível");
      }

      return $"Sucesso: OS [ID: {os.id}] finalizada e veículo {veiculo?.placa} atualizado para Disponível.";
    }
    public List<OrdemDeServico> ListarPo()
        {
      return _ordensDeServico
              .Where(os => os.status != "Excluída")
              .ToList();
        }
  }
}
